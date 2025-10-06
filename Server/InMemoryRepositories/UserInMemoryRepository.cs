using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public UserInMemoryRepository()
    {
        new User("abe", "abeabe");
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        var existing = users.SingleOrDefault(u => u.Id == user.Id);
        if (existing is null)
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        users.Remove(existing);
        users.Add(user);
        //lol? Vil gerne returnere en User når man opdatere den, men kan man bare cast den?
        return (Task<User>)Task.CompletedTask;
    }

    public Task DeleteAsync(string username)
    {
        var toRemove = users.SingleOrDefault(u => u.Username == username);
        if (toRemove is null)
            throw new InvalidOperationException(
                $"User with ID '{username}' not found");
        users.Remove(toRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(string username)
    {
        var user = users.SingleOrDefault(u => u.Username == username);
        if (user is null)
            throw new InvalidOperationException(
                $"User with ID '{username}' not found");
        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}