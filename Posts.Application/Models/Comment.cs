namespace Posts.Application.Models;

public class Comment
{
    public required Guid CommentId { get; init; }
    public required string Text { get; init; }
    public required Guid PostId { get; init; }
}