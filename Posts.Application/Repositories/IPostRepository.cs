using Posts.Application.Models;

namespace Posts.Application.Repositories;

public interface IPostRepository
{
    Task<bool> InitDb();
    Task<IEnumerable<Post>> GetAllAsync();
    Task<bool> CreateAsync(Post post);
    Task<bool> UpdateAsync(Post post);
    Task<Post?> GetByIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid id);
}