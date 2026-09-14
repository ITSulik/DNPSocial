using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> CreateCommentAsync(Comment comment);
    Task<Comment> GetCommentAsync(int id);
    IQueryable<Comment> GetCommentsAsync();
    Task<Comment> UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
}