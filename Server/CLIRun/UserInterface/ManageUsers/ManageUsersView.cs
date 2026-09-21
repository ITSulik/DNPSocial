using RepositoryContracts;

namespace CLIRun.UserInterface.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;

    public ManageUsersView(IUserRepository userRep)
    {
        this.userRepository = userRep;
    }

    public async Task ShowAsync()
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.WriteLine();
            Console.WriteLine("1) Create user");
            Console.WriteLine("2) List users");
            Console.WriteLine("0) Back");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await ListUsersAsync();
                    break;
                case "0":
                    keepRunning = false;
                    break;
                default:
                    Console.WriteLine("Unknown choice.");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        CreateUserView createUserView = new CreateUserView(userRepository);
        await createUserView.ShowAsync();
    }

    private async Task ListUsersAsync()
    {
        ListUsersView listUsersView = new ListUsersView(userRepository);
        await listUsersView.ShowAsync();
    }
}