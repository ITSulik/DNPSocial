using RepositoryContracts;

namespace CLIRun.UserInterface.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public ManagePostsView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.WriteLine();
            Console.WriteLine("1) Create Post");
            Console.WriteLine("2) List Posts");
            Console.WriteLine("3) Show Single Post");
            Console.WriteLine("0) Back");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    await ListPostsAsync();
                    break;
                case "3":
                    await SinglePostAsync();
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

    private async Task SinglePostAsync()
    {
        SinglePostView singlePostView = new(postRepository, commentRepository);
        await singlePostView.ShowAsync();
    }

    private async Task ListPostsAsync()
    {
        ListPostsView listPostsView = new(postRepository);
        await listPostsView.ShowAsync();
    }

    private async Task CreatePostAsync()
    {
        CreatePostView createPostView = new(postRepository);
        await createPostView.ShowAsync();
    }
}