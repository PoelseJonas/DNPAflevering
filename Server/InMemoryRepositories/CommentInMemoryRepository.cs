using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        new Comment("stupid ass post", 1,1 );
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task<Comment> UpdateAsync(Comment comment)
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
        return (Task<Comment>)Task.CompletedTask;
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