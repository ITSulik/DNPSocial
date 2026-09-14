using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> CreatePostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int Id);
    Task<Post> GetPostAsync(int Id);
    IQueryable<Post> GetPostsAsync();
}