namespace ApiContracts_DTO;

public class UpdateCommentDto
{
    //Når du opretter en ny kommentar, skal klienten ikke sende Id (det genereres af databasen).
    public required string Body { get; set; }
}