using RepositoryContracts;

namespace CLIRun.UserInterface.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    
    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        foreach (var post in postRepository.GetPostsAsync())
        {
            Console.WriteLine($"{post.Id} - {post.Title}");
        }
    }
}