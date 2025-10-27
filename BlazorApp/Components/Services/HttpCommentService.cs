using System.Text.Json;
using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    
    
    public async Task<CreateCommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CreateCommentDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task UpdateCommentAsync(CommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"comments/{request.id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var updatedPost = await httpResponse.Content.ReadFromJsonAsync<CommentDto>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        if (updatedPost is null)
        {
            throw new Exception("Empty response body for updatePost");
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<IEnumerable<CommentDto>> GetSingleAsync(int postId)
    {
        var comments = await client.GetFromJsonAsync<List<CommentDto>>(
            $"comments?postId={postId}",
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (comments is null)
        {
            throw new Exception("API returned an empty response body for GetSingle (comments by post).");
        }

        return comments;
    }

    public Task<IEnumerable<CommentDto>> GetMany()
    {
        throw new NotImplementedException();
    }
}