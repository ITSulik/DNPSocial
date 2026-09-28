using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepo : IUserRepository
{
   private readonly List<User> users = new();
   
   public Task<User> CreateUserAsync(User user)
   {
      user.ID = users.Any()
         ? users.Max(u => u.ID) + 1
         : 1;
      users.Add(user);
      return Task.FromResult(user);
   }
   
   public Task UpdateUserAsync(User user)
   {
      User? userToUpdate = users.FirstOrDefault(u => u.ID == user.ID);
      if (userToUpdate is null)
      {
         throw  new InvalidOperationException(
            $"User with ID '{user.ID}' not found");
      }
      
      users.Remove(userToUpdate);
      users.Add(user);
      return Task.CompletedTask;
   }

   public Task DeleteUserAsync(int userId)
   {
      User? userToDelete = users.FirstOrDefault(u => u.ID == userId);
      if (userToDelete is null)
      {  
            throw  new InvalidOperationException(
               $"User with ID '{userId}' not found"); }
      users.Remove(userToDelete);
      return Task.CompletedTask;
   }

   public Task<User> GetUserAsync(int userId)
   {
      User?  user = users.FirstOrDefault(u => u.ID == userId);
      if (user is null)
         {
         throw  new InvalidOperationException(
            $"User with ID '{userId}' not found");
         }
      
      return Task.FromResult(user);
   }

   public Task<User> GetUserByEmailAsync(string email)
   {
      User?  user = users.FirstOrDefault(u => u.Email == email);
      if (user is null)
      {
         throw  new InvalidOperationException(
            $"User with Email '{email}' not found");
      }
      
      return Task.FromResult(user);
   }
   public IQueryable<User> GetUsersAsync()
   {
      return users.AsQueryable();
   }
   // public UserInMemoryRepo()
   // {
   //    SeedData();
   // }
   //
   // private void SeedData()
   // {
   //    users.Add(new User { ID = 1, Username = "sulik", Email = "sulik@example.com", Password = "soulful", Created = DateTime.Now });
   //    users.Add(new User { ID = 2, Username = "Sam", Email = "s4ms3pi0l@example.com", Password = "areallylongpasswordsupposedly", Created = DateTime.Now });
   //    users.Add(new User { ID = 3, Username = "3rdguy", Email = "guy3@example.com", Password = "password123", Created = DateTime.Now });
   // }
}