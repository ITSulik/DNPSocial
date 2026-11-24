using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("posts")]
public class PostsController(IPostRepository postRepository, IUserRepository userRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto request)
    {
        try
        {
            await userRepository.GetUserAsync(request.UserId); // throws if author doesn't exist

            Post post = new()
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId,
                Created = DateTime.Now
            };

            Post created = await postRepository.CreatePostAsync(post);
            PostDto dto = await ToDtoAsync(created);
            return Created($"/posts/{dto.Id}", dto);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest($"Cannot create post: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post existing = await postRepository.GetPostAsync(id);
            existing.Title = request.Title;
            existing.Body = request.Body;
            await postRepository.UpdatePostAsync(existing);
            return Ok(await ToDtoAsync(existing));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        try
        {
            Post post = await postRepository.GetPostAsync(id);
            return Ok(await ToDtoAsync(post));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetMany([FromQuery] string? title, [FromQuery] int? userId, [FromQuery] string? username)
    {
        IQueryable<Post> posts = postRepository.GetPostsAsync();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.ToLower().Contains(title.ToLower()));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            // Uses the User navigation property from 7.5; EF turns it into a JOIN on Users.
            posts = posts.Where(p => p.User.Username.ToLower().Contains(username.ToLower()));
        }

        // Converting to PostDto inside the query lets EF fetch each post's author
        // in the same SQL statement, instead of one extra query per post.
        List<PostDto> dtos = await posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId,
            AuthorUsername = p.User.Username,
            Created = p.Created
        }).ToListAsync();

        return Ok(dtos);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepository.DeletePostAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private async Task<PostDto> ToDtoAsync(Post post)
    {
        User? author = await userRepository.GetUsersAsync()
            .FirstOrDefaultAsync(u => u.ID == post.UserId);

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            AuthorUsername = author?.Username,
            Created = post.Created
        };
    }
}