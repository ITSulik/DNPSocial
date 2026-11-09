namespace ApiContracts;

public class CreateCommentDto
{
    public required string Body { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
}

public class UpdateCommentDto
{
    public required string Body { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string? AuthorUsername { get; set; }
    public DateTime Created { get; set; }
}