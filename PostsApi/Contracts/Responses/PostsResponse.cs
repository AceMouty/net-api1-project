namespace PostsApi.Contracts.Responses;

public class PostsResponse
{
    public required IEnumerable<PostResponse> Posts { get; init; } = Enumerable.Empty<PostResponse>();
}