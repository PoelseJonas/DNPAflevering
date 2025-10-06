using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository: IUserRepository
{
    private readonly string filePath = "comments.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }


    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetSingleAsync(string username)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetMany()
    {
        throw new NotImplementedException();
    }
}