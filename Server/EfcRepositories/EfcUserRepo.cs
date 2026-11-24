using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepo : IUserRepository
{
    private readonly AppDbContext ctx;

    public EfcUserRepo(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        EntityEntry<User> entityEntry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateUserAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(u => u.ID == user.ID))
        {
            throw new InvalidOperationException($"User with ID '{user.ID}' not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.ID == userId);
        if (existing is null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetUserAsync(int userId)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.ID == userId);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        User? user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            throw new InvalidOperationException($"User with Email '{email}' not found");
        }

        return user;
    }

    public IQueryable<User> GetUsersAsync()
    {
        return ctx.Users.AsQueryable();
    }
}