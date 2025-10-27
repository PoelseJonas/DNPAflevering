using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    public Task DeleteUserAsync(int id);
    Task<UserDto> GetSingleAsync(int id);
    Task<IQueryable<UserDto>> GetMany();
}