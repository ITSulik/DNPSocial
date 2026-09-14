using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepo : ICommentRepository
{
    private readonly List<Comment> comments;
    
    public Task<Comment> CreateCommentAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(p => p.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task<Comment> GetCommentAsync(int id)
    {
        Comment?  comment = comments.FirstOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw  new InvalidOperationException($"Comment with id {id} not found");
        }
        
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetCommentsAsync()
    {
        return comments.AsQueryable();
    }

    public Task<Comment> UpdateCommentAsync(Comment comment)
    {
        Comment?  existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw  new InvalidOperationException($"Comment with id {comment.Id} not found");
        }
        existingComment.Body = comment.Body;
        return Task.FromResult(existingComment);
    }

    public Task DeleteCommentAsync(int id)
    {
        Comment? comment = comments.FirstOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with id {comment.Id} not found");
        }
        comments.Remove(comment);
        return Task.CompletedTask;
    }
}