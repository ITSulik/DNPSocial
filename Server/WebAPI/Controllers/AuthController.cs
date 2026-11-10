using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IUserRepository userRepository) : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<UserDto> Login([FromBody] LoginRequest request)
    {
        User? user = userRepository.GetUsersAsync()
            .FirstOrDefault(u => u.Username == request.Username);

        if (user is null || user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(ToDto(user));
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto { Id = user.ID, Username = user.Username, Email = user.Email, Created = user.Created };
    }
}