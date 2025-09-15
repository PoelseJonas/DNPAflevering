using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        new Comment{ Id = 1, Body= "Stupid ass post", UserId = 2};
        new Comment{ Id = 2, Body= "This post is fucking genius", UserId = 1};
        new Comment{ Id = 3, Body= "im dumbo", UserId = 3};
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existing =
            comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existing is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existing);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var toRemove = comments.SingleOrDefault(c => c.Id == id);
        if (toRemove is null)
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        comments.Remove(toRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        var comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}