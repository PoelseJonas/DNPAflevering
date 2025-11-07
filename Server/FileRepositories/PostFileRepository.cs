using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? existingPost = posts.FirstOrDefault(u => u.Id == post.Id);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"User with ID '{post.Id}' not found.");
        }
        
        // Update the properties of the existing user
        existingPost.Title = post.Title;
        existingPost.Body = post.Body;
        // andre ting der skal ændres

        // Write the updated list back to the file
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);

        return existingPost;
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        
        Post? post = posts.FirstOrDefault(post => post.Id == id);
        if (post == null)
        {
            throw new Exception($"Post with title '{post.Title}' not found.");
        }
        
        posts.Remove(post);
        
        //returner den opdaterede liste til filen
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
       
        Post? post = posts.FirstOrDefault(post => post.Id == id);
        
        if (post == null)
        {
            throw new Exception($"Post with title '{post.Title}' not found.");
        }
        
        return post;
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }
}