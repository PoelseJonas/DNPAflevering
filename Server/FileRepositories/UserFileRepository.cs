using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }


    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        User? existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID '{user.Id}' not found.");
        }
        
        // Update the properties of the existing user
        existingUser.Username = user.Username;
        // andre ting der skal ændres

        // Write the updated list back to the file
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);

        return existingUser;
    }

    public async Task DeleteAsync(string username)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        User? user = users.FirstOrDefault(user => user.Username == username);
        if (user == null)
        {
            throw new Exception($"User with username '{username}' not found.");
        }
        
        users.Remove(user);
        
        //returner den opdaterede liste til filen
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }

    public async Task<User> GetSingleAsync(string username)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
       
        User? user = users.FirstOrDefault(user => user.Username == username);
        
        if (user == null)
        {
            throw new Exception($"User with username '{username}' not found.");
        }
        
        return user;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }
}