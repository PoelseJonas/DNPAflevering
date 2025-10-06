using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(string username);
    Task<User> GetSingleAsync(string username);
    IQueryable<User> GetMany();
}

/*
 * Add takes a Post, and returns the created Post. This is because the server sets some data on the Post, e.g. the ID, and this should be returned to the client. The client might need this ID for something. This is common.
 * Update takes a Post (with ID), and just replaces the existing Post. If no existing Post is found, an exception is thrown to indicate the error.
 * Delete will remove the Post with a given ID. If no matching Post is found, an exception is thrown.
 * GetSingle will return the Post matching the given ID. If no Post is found, an exception is thrown.
 * GetMany will return an IQueryable. This is an interface which can looped over in a for-each loop to extract the relevant entities. Or we can use LINQ, which we will see later in the course. This makes filtering the Posts by some criteria easier (using predicates, later). Maybe we want to fetch all Posts with a specific sub-string in the postname. Or some other property on the Post. The method is not async, the reason of which we will get back to, when we add a database.

*/