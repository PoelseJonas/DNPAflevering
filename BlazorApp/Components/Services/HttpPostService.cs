using System.Text.Json;
using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public class HttpPostService: IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        Console.WriteLine(request);
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var updatedPost = await httpResponse.Content.ReadFromJsonAsync<PostDto>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        if (updatedPost is null)
        {
            throw new Exception("Empty response body for updatePost");
        }
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<PostDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var post = await httpResponse.Content.ReadFromJsonAsync<PostDto>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (post is null)
        {
            throw new Exception(
                "API returned an empty response body for GetSingle.");
        }

        return post;
    }

    public async Task<IQueryable<PostDto>> GetMany()
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"posts");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var posts = await httpResponse.Content.ReadFromJsonAsync<List<PostDto>>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (posts is null)
        {
            throw new Exception(
                "API returned an empty response body for GetSingle.");
        }

        return posts.AsQueryable();

    }
}