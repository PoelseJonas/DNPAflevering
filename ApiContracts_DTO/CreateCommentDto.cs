namespace ApiContracts_DTO;

public class CreateCommentDto
{
    //Når du opretter en ny kommentar, skal klienten ikke sende Id (det genereres af databasen).
    public required string Body { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
}