using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
            CommentDto dto = ToDto(created);
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
            return Ok(ToDto(existing));
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
            return Ok(ToDto(comment));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<CommentDto>> GetMany([FromQuery] int? postId, [FromQuery] int? userId, [FromQuery] string? username)
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
            List<int> matchingUserIds = userRepository.GetUsersAsync()
                .Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.ID)
                .ToList();
            comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
        }

        return Ok(comments.Select(ToDto).ToList());
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

    private static CommentDto ToDto(Comment comment)
    {
        return new CommentDto { Id = comment.Id, Body = comment.Body, UserId = comment.UserId, PostId = comment.PostId, Created = comment.Created };
    }
}