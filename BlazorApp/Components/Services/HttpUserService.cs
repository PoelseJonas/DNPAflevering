using System.Text.Json;
using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        Console.WriteLine(request);
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UserDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var updatedUser = await httpResponse.Content.ReadFromJsonAsync<UserDto>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        if (updatedUser is null)
        {
            throw new Exception("Empty response body for updateUser");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var user = await httpResponse.Content.ReadFromJsonAsync<UserDto>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (user is null)
        {
            throw new Exception(
                "API returned an empty response body for GetSingle.");
        }

        return user;
    }

    public async Task<IQueryable<UserDto>> GetMany()
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var users = await httpResponse.Content.ReadFromJsonAsync<List<UserDto>>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (users is null)
        {
            throw new Exception(
                "API returned an empty response body for GetSingle.");
        }

        return users.AsQueryable();

    }

}