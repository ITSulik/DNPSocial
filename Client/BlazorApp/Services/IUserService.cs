using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> GetUserAsync(int userId);
    public Task<UserDto> CreateUserAsync(CreateUserDto request);
    public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request);
    public Task DeleteUserAsync(int userId);
    public Task<List<UserDto>> GetUsersAsync(string? username = null);
}