using ApiContracts_DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    private void VerifyUserNameIsAvailable(string username)
    {

        var existingUser = userRepository.GetMany().FirstOrDefault(u=> u.Username == username);
        if (existingUser != null)
        {
            throw new Exception("Username is already taken.");
        }
    }
    
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            VerifyUserNameIsAvailable(request.UserName);

            User user = new(request.UserName, request.Password);
            User created = await userRepository.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.Username
            };
            Console.WriteLine(user);
            return Created($"/Users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //Cacheable
    private static Dictionary<string, (User user, DateTime timeStamp)> userCache =
        new Dictionary<string, (User user, DateTime timeStamp)>();

    //Hvor lang tid den skal caches: (60 sekunder)
    private static readonly TimeSpan cacheDuration = TimeSpan.FromSeconds(60);
    
    //Get bruger
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserByName(string username)
    {
        if (userCache.ContainsKey(username))
        {
            var (cachedUser, cacheTime) = userCache[username];

            //Her tjekker man om tiden er udløbet eller ej
            if (DateTime.Now - cacheTime < cacheDuration)
            {
                var dtoCached = new UserDto
                {
                    Id = cachedUser.Id,
                    UserName = cachedUser.Username,
                };
                //Tiden er ikke udløbet, så den er stadig "cached"
                return Ok(new
                {
                    cached = true,
                    user = dtoCached,
                });
            }
        }
        //Hvis brugeren ikke er cached eller tiden er løbet ud:

        var user = await userRepository.GetSingleAsync(username);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var dto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username
        };

        // Store the post in the cache with the current timestamp
        userCache[username] = (user, DateTime.Now);

        return Ok(new
        {
            cached = false,
            user = dto
        });
    }
    
    // PUT/Update endpoint to update an existing user
    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserDto dto)
    {
        var user = await userRepository.GetSingleAsync(username);
        
        if (user is null)
        {
            return NotFound("User not found");
        } 
        //update brugeren:
       user.Username = dto.UserName;
       user.Password = dto.Password;

       var updated = await userRepository.UpdateAsync(user);
       
       //opdater cachen
       userCache[username] = (updated, DateTime.Now);

       return Ok(new UserDto
       {
           Id = updated.Id,
           UserName = updated.Username
       });
    }
    
    // DELETE endpoint to delete a user by username
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await userRepository.GetSingleAsync(username);
        if (user is null)
        {
            return NotFound("Post not found");
        }

        await userRepository.DeleteAsync(username);
        userCache.Remove(username);
        
        return NoContent();
    }
    
    //Alle get For the GetMany users endpoint, it must be possible to-
    //filter the users by whether the username contains a certain string.
    //You may add more filter criteria.
    [HttpGet]
    public ActionResult<IEnumerable<User>> getAllUsers()
    {
        //return Ok(userRepository.GetMany());
        var users = userRepository.GetMany()
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.Username
            });

        return Ok(users);
    }
}