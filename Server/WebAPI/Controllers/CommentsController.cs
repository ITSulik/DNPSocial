using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("comments")]
public class CommentsController(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto request)
    {
        try
        {
            await postRepository.GetPostAsync(request.PostId);
            await userRepository.GetUserAsync(request.UserId);

            Comment comment = new()
            {
                Body = request.Body,
                UserId = request.UserId,
                PostId = request.PostId,
                Created = DateTime.Now
            };

            Comment created = await commentRepository.CreateCommentAsync(comment);
            CommentDto dto = await ToDtoAsync(created);
            return Created($"/comments/{dto.Id}", dto);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest($"Cannot create comment: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment existing = await commentRepository.GetCommentAsync(id);
            existing.Body = request.Body;
            await commentRepository.UpdateCommentAsync(existing);
            return Ok(await ToDtoAsync(existing));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            Comment comment = await commentRepository.GetCommentAsync(id);
            return Ok(await ToDtoAsync(comment));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetMany([FromQuery] int? postId, [FromQuery] int? userId, [FromQuery] string? username)
    {
        IQueryable<Comment> comments = commentRepository.GetCommentsAsync();

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            // Uses the User navigation property from 7.5; EF turns it into a JOIN on Users.
            comments = comments.Where(c => c.User.Username.ToLower().Contains(username.ToLower()));
        }

        // Converting to CommentDto inside the query lets EF fetch each comment's author
        // in the same SQL statement, instead of one extra query per comment.
        List<CommentDto> dtos = await comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId,
            AuthorUsername = c.User.Username,
            Created = c.Created
        }).ToListAsync();

        return Ok(dtos);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepository.DeleteCommentAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private async Task<CommentDto> ToDtoAsync(Comment comment)
    {
        User? author = await userRepository.GetUsersAsync()
            .FirstOrDefaultAsync(u => u.ID == comment.UserId);

        return new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId,
            AuthorUsername = author?.Username,
            Created = comment.Created
        };
    }
}