using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> GetUserAsync(int userId);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
    IQueryable<User> GetUsersAsync();
}