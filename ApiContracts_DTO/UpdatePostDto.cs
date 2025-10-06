namespace ApiContracts_DTO;

public class UpdatePostDto
{
    //Når du opdaterer en post, skal klienten normalt ikke kunne ændre Id, kun indholdet.
    public required string Title { get; set; }
    public required string Body { get; set; }
}