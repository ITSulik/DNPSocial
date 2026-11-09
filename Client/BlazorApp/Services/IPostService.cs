using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> CreatePostAsync(CreatePostDto request);
    public Task<PostDto> UpdatePostAsync(int id, UpdatePostDto request);
    public Task DeletePostAsync(int Id);
    public Task<PostDto> GetPostAsync(int Id);
    public Task<List<PostDto>> GetPostsAsync();
}