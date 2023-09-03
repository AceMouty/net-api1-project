using System.Runtime.InteropServices.JavaScript;
using Posts.Application.Models;
using PostsApi.Contracts.Requests;
using PostsApi.Contracts.Responses;

namespace PostsApi.Mapping;

public static class ContractMapping
{
    public static PostResponse MapToResponse(this Post post)
    {
        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Contents = post.Contents
        };
    }
    public static PostsResponse MapToResponse(this IEnumerable<Post> posts)
    {
        return new PostsResponse
        {
            Posts = posts.Select(MapToResponse)
        };
    }

    public static Post MapToPost(this CreatePostRequest post)
    {
        var utcDate = DateTime.Now.ToUniversalTime();
        return new Post
        {
            Id = Guid.NewGuid(),
            Title = post.Title,
            Contents = post.Contents,
            CreatedAt = utcDate,
            UpdatedAt = utcDate
        };
    }

    public static Post MapToPost(this UpdatePostRequest post, Guid id)
    {
        return new Post()
        {
            Id = id,
            Title = post.Title,
            Contents = post.Contents,
            CreatedAt = default,
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
    }
}