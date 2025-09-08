using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(u => u.UserId) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        var existing = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existing is null)
            throw new InvalidOperationException(
                $"User with ID '{user.UserId}' not found");
        users.Remove(existing);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var toRemove = users.SingleOrDefault(u => u.UserId == id);
        if (toRemove is null)
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        users.Remove(toRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        var user = users.SingleOrDefault(u => u.UserId == id);
        if (user is null)
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}