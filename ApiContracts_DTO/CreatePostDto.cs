namespace ApiContracts_DTO;

public class CreatePostDto
{
    //Når du opretter et nyt Post, skal klienten ikke sende Id (det genereres af databasen).
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; }
}