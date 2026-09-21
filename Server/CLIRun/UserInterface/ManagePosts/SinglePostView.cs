using RepositoryContracts;

namespace CLIRun.UserInterface.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Post ID: ");
        int postId = int.Parse(Console.ReadLine() ?? "0");

        var post = await postRepository.GetPostAsync(postId);

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine("Comments:");

        var commentsOnPost = commentRepository.GetCommentsAsync().Where(c => c.PostId == postId);

        if (!commentsOnPost.Any())
        {
            Console.WriteLine("  (no comments yet)");
        }

        foreach (var comment in commentsOnPost)
        {
            Console.WriteLine($"  - {comment.Body} (user {comment.UserId})");
        }
    }
}