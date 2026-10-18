using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
            PostDto dto = ToDto(created);
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
            return Ok(ToDto(existing));
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
            return Ok(ToDto(post));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<PostDto>> GetMany([FromQuery] string? title, [FromQuery] int? userId, [FromQuery] string? username)
    {
        IQueryable<Post> posts = postRepository.GetPostsAsync();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            List<int> matchingUserIds = userRepository.GetUsersAsync()
                .Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.ID)
                .ToList();
            posts = posts.Where(p => matchingUserIds.Contains(p.UserId));
        }

        return Ok(posts.Select(ToDto).ToList());
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

    private static PostDto ToDto(Post post)
    {
        return new PostDto { Id = post.Id, Title = post.Title, Body = post.Body, UserId = post.UserId, Created = post.Created };
    }
}