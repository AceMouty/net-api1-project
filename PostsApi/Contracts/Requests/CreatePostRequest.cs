namespace PostsApi.Contracts.Requests;

public class CreatePostRequest
{
    public required string Title { get; init; }
    public required string Contents { get; init; }
}