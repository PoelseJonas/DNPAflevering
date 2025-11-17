using ApiContracts_DTO;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and password are required.");
        
        

        var user = await userRepository.GetSingleAsync(request.Username);
        if (user.Password != request.Password)
            return Unauthorized("Invalid credentials.");
        
        
        
        
        
        // Map the user entity to the DTO so no sensitive fields (like password) are returned
        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username,
        };

        return Ok(userDto);
        
    }
    
    
}