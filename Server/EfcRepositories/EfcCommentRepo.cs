using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepo : ICommentRepository
{
    private readonly AppDbContext ctx;

    public EfcCommentRepo(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<Comment> GetCommentAsync(int id)
    {
        Comment? comment = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return comment;
    }

    public IQueryable<Comment> GetCommentsAsync()
    {
        return ctx.Comments.AsQueryable();
    }

    public async Task<Comment> UpdateCommentAsync(Comment comment)
    {
        if (!await ctx.Comments.AnyAsync(c => c.Id == comment.Id))
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteCommentAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }
}