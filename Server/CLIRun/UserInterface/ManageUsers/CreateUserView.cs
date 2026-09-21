using Entities;
using RepositoryContracts;

namespace CLIRun.UserInterface.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRep)
    {
        this.userRepository = userRep;
    }

    public async Task ShowAsync()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        bool usernameTaken = userRepository.GetUsersAsync().Any(u => u.Username == username);
        if (usernameTaken)
        {
            Console.WriteLine($"Username '{username}' is already taken.");
            return;
        }

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        User user = new User
        {
            Username = username,
            Email = email,
            Password = password,
            Created = DateTime.Now
        };

        User created = await userRepository.CreateUserAsync(user);
        Console.WriteLine($"User created with ID {created.ID}");
    }
}