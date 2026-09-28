using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileStorage;

public class UserFileStorage : IUserRepository
{
    private readonly string filePath = "users.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public UserFileStorage()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(SeedData(), JsonOptions));
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        List<User> users = await LoadAsync();
        user.ID = users.Count > 0 ? users.Max(u => u.ID) + 1 : 1;
        users.Add(user);
        await SaveAsync(users);
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        List<User> users = await LoadAsync();
        int index = users.FindIndex(u => u.ID == user.ID);
        if (index == -1)
        {
            throw new InvalidOperationException($"User with ID '{user.ID}' not found");
        }

        users[index] = user;
        await SaveAsync(users);
    }

    public async Task DeleteUserAsync(int userId)
    {
        List<User> users = await LoadAsync();
        User? userToDelete = users.FirstOrDefault(u => u.ID == userId);
        if (userToDelete is null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        users.Remove(userToDelete);
        await SaveAsync(users);
    }

    public async Task<User> GetUserAsync(int userId)
    {
        List<User> users = await LoadAsync();
        User? user = users.FirstOrDefault(u => u.ID == userId);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        List<User> users = await LoadAsync();
        User? user = users.FirstOrDefault(u => u.Email == email);
        if (user is null)
        {
            throw new InvalidOperationException($"User with Email '{email}' not found");
        }

        return user;
    }

    public IQueryable<User> GetUsersAsync()
    {
        return LoadAsync().Result.AsQueryable();
    }

    private async Task<List<User>> LoadAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<User>>(json)!;
    }

    private async Task SaveAsync(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, JsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    private static List<User> SeedData()
    {
        return new List<User>
        {
            new User { ID = 1, Username = "sulik", Email = "sulik@example.com", Password = "soulful", Created = DateTime.Now },
            new User { ID = 2, Username = "Sam", Email = "s4ms3pi0l@example.com", Password = "areallylongpasswordsupposedly", Created = DateTime.Now },
            new User { ID = 3, Username = "3rdguy", Email = "guy3@example.com", Password = "password123", Created = DateTime.Now }
        };
    }
}