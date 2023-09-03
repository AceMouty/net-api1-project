using System.Runtime.InteropServices.JavaScript;
using Posts.Application.Models;

namespace Posts.Application.Repositories;

public class PostRepository : IPostRepository
{
    // in memory DB
    private readonly List<Post> _posts = new();
    private readonly List<Comment> _comments = new();

    public Task<bool> InitDb()
    {
        var p = new Post
        {
            Id = Guid.NewGuid(),
            Title = ".NET Is The Best",
            Contents = ".Net is the best...that is all",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var c = new Comment
        {
            CommentId = Guid.NewGuid(),
            Text = "This was awesome!",
            PostId = p.Id
        };
        
        _posts.Add(p);
        _comments.Add(c);
        
        return Task.FromResult(true);
    }
    public Task<IEnumerable<Post>> GetAllAsync()
    {
        return Task.FromResult(_posts.AsEnumerable());
    }

    public Task<bool> CreateAsync(Post post)
    {
        _posts.Add(post);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateAsync(Post updatedPost)
    {
        var postIndex = _posts.FindIndex(p => p.Id == updatedPost.Id);
        if (postIndex == -1)
        {
            return Task.FromResult(false);
        }

        var post = _posts[postIndex];
        Post p = new()
        {
            Id = post.Id,
            Title = updatedPost.Title,
            Contents = updatedPost.Contents,
            CreatedAt = post.CreatedAt,
            UpdatedAt = updatedPost.UpdatedAt
        };

        _posts[postIndex] = p;
        return Task.FromResult(true);
    }

    public Task<Post?> GetByIdAsync(Guid id)
    {
        var post = _posts.SingleOrDefault(p => p.Id == id);
        return Task.FromResult(post);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _posts.RemoveAll(p => p.Id == id);
        var movieWasRemoved = removedCount > 0;
        return Task.FromResult(movieWasRemoved);
    }

    public Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid id)
    {
        var comments = _comments.Where(c => c.PostId == id);
        return Task.FromResult(comments);
    }
}