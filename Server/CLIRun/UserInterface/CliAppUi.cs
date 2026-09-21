using RepositoryContracts;
using CLIRun.UserInterface.ManagePosts;
using CLIRun.UserInterface.ManageUsers;
using CLIRun.UserInterface.ManageComments;

namespace CLIRun.UserInterface;

public class CliAppUi
{
    private readonly ManageUsersView _manageUsersView;
    private readonly ManagePostsView _managePostsView;
    private readonly ManageCommentsView _manageCommentsView;

    public CliAppUi(IUserRepository userRepository, IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        _manageUsersView = new ManageUsersView(userRepository);
        _managePostsView = new ManagePostsView(postRepository, commentRepository);
        _manageCommentsView = new ManageCommentsView(commentRepository, postRepository, userRepository);
    }

    public async Task StartAsync()
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.WriteLine();
            Console.WriteLine("1) Manage users");
            Console.WriteLine("2) Manage posts");
            Console.WriteLine("3) Manage comments");
            Console.WriteLine("0) Exit");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await _manageUsersView.ShowAsync();
                    break;
                case "2":
                    await _managePostsView.ShowAsync();
                    break;
                case "3":
                    await _manageCommentsView.ShowAsync();
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
}