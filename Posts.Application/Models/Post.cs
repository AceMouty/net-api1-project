namespace Posts.Application.Models;

public class Post
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Contents { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}