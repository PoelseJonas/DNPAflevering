namespace ApiContracts_DTO;

public class CommentDto
{
    public required string Body { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
    public required int id { get; set; }
}