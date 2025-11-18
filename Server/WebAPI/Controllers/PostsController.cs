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
                Title = created.Title,
                UserId = created.UserId
            };
            Console.WriteLine("mvi er nu i webapi" + dto);
            return Created($"/Posts/{dto.Id}", dto); ;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
        
    }

  
    //specifik post
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id)
    {
      var post = await postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        var dto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };
        
        return Ok(dto);
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
            Body = p.Body,
            UserId = p.UserId
        });

        return Ok(postDtos);
    }
}