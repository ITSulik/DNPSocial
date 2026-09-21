using Entities;
using RepositoryContracts;

namespace CLIRun.UserInterface.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreateCommentView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Post ID to comment on: ");
        int postId = int.Parse(Console.ReadLine() ?? "0");

        try
        {
            await postRepository.GetPostAsync(postId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"No post with ID {postId} exists.");
            return;
        }

        Console.Write("Your user ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        try
        {
            await userRepository.GetUserAsync(userId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"No user with ID {userId} exists.");
            return;
        }

        Console.Write("Comment: ");
        string body = Console.ReadLine() ?? "";

        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId,
            Created = DateTime.Now
        };

        Comment created = await commentRepository.CreateCommentAsync(comment);
        Console.WriteLine($"Comment created with ID {created.Id}");
    }
}