namespace PostsApi.Contracts.Responses;

public class PostResponse
{
    public required Guid Id { get; set; }
    public required string Title { get; init; }
    public required string Contents { get; set; }
}