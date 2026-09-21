using RepositoryContracts;

namespace CLIRun.UserInterface.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRep)
    {
        this.userRepository = userRep;
    }

    public async Task ShowAsync()
    {
        
        foreach (var user in userRepository.GetUsersAsync())
        {
            Console.WriteLine($"{user.ID} - {user.Username}");
        }
    }
}