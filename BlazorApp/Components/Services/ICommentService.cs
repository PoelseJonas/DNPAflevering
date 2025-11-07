using ApiContracts_DTO;

namespace BlazorApp.Components.Services;

public interface ICommentService
{
    public Task<CreateCommentDto> AddCommentAsync(CreateCommentDto request);
    public Task UpdateCommentAsync(CommentDto request);
    public Task DeleteCommentAsync(int id);
    public Task<IQueryable> GetSingleAsync(int postId);
    public Task<IQueryable<CommentDto>> GetMany();
}