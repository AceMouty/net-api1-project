namespace PostsApi.Contracts.Requests;

public class UpdatePostRequest
{
    public required string Title { get; init; }
    public required string Contents { get; init; }
}