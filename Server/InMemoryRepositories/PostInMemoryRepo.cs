using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepo : IPostRepository
{
    private readonly List<Post> posts = new();
    public Task<Post> CreatePostAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdatePostAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeletePostAsync(int Id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == Id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{Id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetPostAsync(int Id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == Id);
        if (post is null)
        { throw new InvalidOperationException(
                $"Post with ID '{Id}' not found");
            }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetPostsAsync()
    {
        return posts.AsQueryable();
    }
}