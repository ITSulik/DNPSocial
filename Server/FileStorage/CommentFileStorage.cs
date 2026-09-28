using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileStorage;

public class CommentFileStorage : ICommentRepository
{
    private readonly string filePath = "comments.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public CommentFileStorage()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(SeedData(), JsonOptions));
        }
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadAsync();
        comment.Id = comments.Count > 0 ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);
        await SaveAsync(comments);
        return comment;
    }

    public async Task<Comment> GetCommentAsync(int id)
    {
        List<Comment> comments = await LoadAsync();
        Comment? comment = comments.FirstOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return comment;
    }

    public IQueryable<Comment> GetCommentsAsync()
    {
        return LoadAsync().Result.AsQueryable();
    }

    public async Task<Comment> UpdateCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadAsync();
        int index = comments.FindIndex(c => c.Id == comment.Id);
        if (index == -1)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }

        comments[index] = comment;
        await SaveAsync(comments);
        return comment;
    }

    public async Task DeleteCommentAsync(int id)
    {
        List<Comment> comments = await LoadAsync();
        Comment? commentToRemove = comments.FirstOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        await SaveAsync(comments);
    }

    private async Task<List<Comment>> LoadAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Comment>>(json)!;
    }

    private async Task SaveAsync(List<Comment> comments)
    {
        string json = JsonSerializer.Serialize(comments, JsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    private static List<Comment> SeedData()
    {
        return new List<Comment>
        {
            new Comment { Id = 1, Body = "Woohoo", UserId = 2, PostId = 1, Created = DateTime.Now },
            new Comment { Id = 2, Body = "First", UserId = 3, PostId = 1, Created = DateTime.Now },
            new Comment { Id = 3, Body = "The Mastermind", UserId = 1, PostId = 2, Created = DateTime.Now }
        };
    }
}