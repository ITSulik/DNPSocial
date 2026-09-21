using RepositoryContracts;

namespace CLIRun.UserInterface.ManageComments;

public class ManageCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public ManageCommentsView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.WriteLine();
            Console.WriteLine("1) Add comment to a post");
            Console.WriteLine("0) Back");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddCommentAsync();
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

    private async Task AddCommentAsync()
    {
        CreateCommentView createCommentView = new(commentRepository, postRepository, userRepository);
        await createCommentView.ShowAsync();
    }
}