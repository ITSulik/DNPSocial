using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileStorage;

public class PostFileStorage : IPostRepository
{
    private readonly string filePath = "posts.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public PostFileStorage()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(SeedData(), JsonOptions));
        }
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        post.Id = posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        await SaveAsync(posts);
        return post;
    }

    public async Task UpdatePostAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        int index = posts.FindIndex(p => p.Id == post.Id);
        if (index == -1)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        posts[index] = post;
        await SaveAsync(posts);
    }

    public async Task DeletePostAsync(int Id)
    {
        List<Post> posts = await LoadAsync();
        Post? postToRemove = posts.FirstOrDefault(p => p.Id == Id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{Id}' not found");
        }

        posts.Remove(postToRemove);
        await SaveAsync(posts);
    }

    public async Task<Post> GetPostAsync(int Id)
    {
        List<Post> posts = await LoadAsync();
        Post? post = posts.FirstOrDefault(p => p.Id == Id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{Id}' not found");
        }

        return post;
    }

    public IQueryable<Post> GetPostsAsync()
    {
        return LoadAsync().Result.AsQueryable();
    }

    private async Task<List<Post>> LoadAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Post>>(json)!;
    }

    private async Task SaveAsync(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(posts, JsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    private static List<Post> SeedData()
    {
        return new List<Post>
        {
            new Post { Id = 1, Title = "Welcome to DNPSocial", Body = "This is the first post on the forum!", UserId = 1, Created = DateTime.Now },
            new Post { Id = 2, Title = "Hello friend", Body = "Hello friend, let's give you a name", UserId = 2, Created = DateTime.Now }
        };
    }
}