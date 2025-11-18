namespace ApiContracts_DTO;

public class PostDto
{
    public required string Body { get; set; }
    public required string Title { get; set; }
    public int Id { get; set; }
    
    public int UserId { get; set; }
}