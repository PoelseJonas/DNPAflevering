using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    public Task UpdatePostAsync(int id, UpdatePostDto request);
    public Task DeletePostAsync(int id);
    public Task<PostDto> GetSingleAsync(int id);
    public Task<IEnumerable<PostDto>> GetMany();
}