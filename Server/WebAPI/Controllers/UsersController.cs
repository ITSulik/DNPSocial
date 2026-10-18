using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto request)
    {
        try
        {
            bool usernameTaken = userRepository.GetUsersAsync().Any(u => u.Username == request.Username);
            if (usernameTaken)
            {
                return BadRequest($"Username '{request.Username}' is already taken.");
            }

            User user = new()
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                Created = DateTime.Now
            };

            User created = await userRepository.CreateUserAsync(user);
            UserDto dto = ToDto(created);
            return Created($"/users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User existing = await userRepository.GetUserAsync(id);
            existing.Username = request.Username;
            existing.Email = request.Email;
            existing.Password = request.Password;
            await userRepository.UpdateUserAsync(existing);
            return Ok(ToDto(existing));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        try
        {
            User user = await userRepository.GetUserAsync(id);
            return Ok(ToDto(user));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<UserDto>> GetMany([FromQuery] string? username)
    {
        IQueryable<User> users = userRepository.GetUsersAsync();

        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(users.Select(ToDto).ToList());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepository.DeleteUserAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto { Id = user.ID, Username = user.Username, Email = user.Email, Created = user.Created };
    }
}