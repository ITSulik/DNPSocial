using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepo : IPostRepository
{
    private readonly AppDbContext ctx;

    public EfcPostRepo(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        EntityEntry<Post> entityEntry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdatePostAsync(Post post)
    {
        if (!await ctx.Posts.AnyAsync(p => p.Id == post.Id))
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetPostAsync(int id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return post;
    }

    public IQueryable<Post> GetPostsAsync()
    {
        return ctx.Posts.AsQueryable();
    }
}