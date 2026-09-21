using Entities;
using RepositoryContracts;

namespace CLIRun.UserInterface.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Body: ");
        string body = Console.ReadLine() ?? "";

        Console.Write("Your user ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId,
            Created = DateTime.Now
        };

        Post created = await postRepository.CreatePostAsync(post);
        Console.WriteLine($"Post created with ID {created.Id}");
    }
}