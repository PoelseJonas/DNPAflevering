using ApiContracts_DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostsController:ControllerBase
{
    private readonly IPostRepository postRepository;

    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            Post post = new(request.Title, request.Body, request.UserId);
            Post created = await postRepository.AddAsync(post);
            
            var dto = new PostDto
            {
                Id = created.Id,
                Body = created.Body,
                Title = created.Title
            };
            return Created($"/Posts/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    //Cacheable
    private static Dictionary<int, (Post post, DateTime timeStamp)> postCache =
        new Dictionary<int, (Post post, DateTime timeStamp)>();

    //Hvor lang tid den skal caches: (60 sekunder)
    private static readonly TimeSpan cacheDuration = TimeSpan.FromSeconds(60);


    //specifik post
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        if (postCache.TryGetValue(id, out var cached))
        {
            var (cachedPost, cacheTime) = postCache[id];

            //Her tjekker man om tiden er udløbet eller ej
            if (DateTime.Now - cacheTime < cacheDuration)
            {
                var dtoCached = new PostDto
                {
                    Id = id,
                    Body = cachedPost.Body,
                    Title = cachedPost.Title
                };
                //Tiden er ikke udløbet, så den er stadig "cached"
                return Ok(new
                {
                    cached = true,
                    post = dtoCached,
                });
            }
        }
        //Hvis posten ikke er cached eller tiden er løbet ud:

        var post = await postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        // Store the post in the cache with the current timestamp
        postCache[id] = (post, DateTime.Now);

        var dto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body
        };
        
        return Ok(new
        {
            cached = false,
            post = dto
        });
    }
    
    // PUT/Update endpoint to update an existing post
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(int id,[FromBody] UpdatePostDto dto)
    {
        var post = await postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        post.Body = dto.Body;
        post.Title = dto.Title;

        
        var updated = await postRepository.UpdateAsync(post);
        postCache[id] = (updated, DateTime.Now);

        return Ok(new PostDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Body = updated.Body
        });
    }
    
    // DELETE endpoint to delete a post by ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        await postRepository.DeleteAsync(id);
        postCache.Remove(id);
        return NoContent();
    }
    
    //Alle get For the GetMany users endpoint, it must be possible to-
    //filter the users by whether the username contains a certain string.
    //You may add more filter criteria.
    [HttpGet]
    public ActionResult<IEnumerable<Post>> getAllPosts([FromQuery] int? id = null)
    {
        var posts = postRepository.GetMany();

        // Hvis der er angivet et id
        if (id.HasValue)
        {
            posts = posts.Where(p => p.Id == id.Value);
        }

        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body
        });

        return Ok(postDtos);
    }
}