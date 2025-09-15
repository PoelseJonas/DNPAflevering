using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new();

    public PostInMemoryRepository()
    {
        new Post
        {
            PostId = 1, Title = "Hello World", Body = "First post!", UserId = 1
        };
        new Post
        {
            PostId = 2, Title = "Second Post", Body = "Another post", UserId = 2
        };
        new Post
        {
            PostId = 3, Title = "Third Post", Body = "Another another post", UserId = 3
        };
    }

    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any()
            ? posts.Max(p => p.PostId) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost =
            posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.PostId}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? postToGet = posts.SingleOrDefault(p => p.PostId == id);
        if (postToGet is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(postToGet);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}