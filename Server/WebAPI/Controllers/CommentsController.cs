using ApiContracts_DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddCommentAsync(
        [FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = new(request.Body, request.UserId, request.PostId);
            Comment created = await commentRepository.AddAsync(comment);
            
            CommentDto dto = new()
            {
                Body = created.Body,
                UserId = created.UserId,
                PostId = created.PostId,
                id = created.Id
            };
            
            return Created($"/Comments/{dto.id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }


    //Cacheable
    private static Dictionary<int, (Comment comment, DateTime timeStamp)>
        commentCache =
            new Dictionary<int, (Comment comment, DateTime timeStamp)>();

    //Hvor lang tid den skal caches: (60 sekunder)
    private static readonly TimeSpan cacheDuration = TimeSpan.FromSeconds(60);

    //Get kommentar
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        if (commentCache.TryGetValue(id, out var cached))
        {
            var (cachedComment, cacheTime) = commentCache[id];

            //Her tjekker man om tiden er udløbet eller ej
            if (DateTime.Now - cacheTime < cacheDuration)
            {
                var dtoCached = new CommentDto
                {
                    Body = cachedComment.Body,
                    UserId = cachedComment.UserId,
                    PostId = cachedComment.PostId,
                    id = cachedComment.Id,
                };
                //Tiden er ikke udløbet, så den er stadig "cached"
                return Ok(new
                {
                    cached = true,
                    comment = cachedComment,
                });
            }
        }
        //Hvis brugeren ikke er cached eller tiden er løbet ud:

        var comment = await commentRepository.GetSingleAsync(id);
        if (id == -1)
        {
            return NotFound("User not found");
        }

        var dto = new CommentDto
        {
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId,
            id = comment.Id,
        };

        // Store the post in the cache with the current timestamp
        commentCache[id] = (comment, DateTime.Now);

        return Ok(new
        {
            cached = false,
            comment = dto
        });
    }

    // PUT/Update endpoint to update an existing user
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateComment(int id,
        [FromBody] UpdateCommentDto dto)
    {
        var comment = await commentRepository.GetSingleAsync(id);

        if (comment is null)
        {
            return NotFound("User not found");
        }

        //update kommentaren:
        comment.Body = dto.Body;

        var updated = await commentRepository.UpdateAsync(comment);

        //opdater cachen
        commentCache[id] = (updated, DateTime.Now);

        
        return Ok(new CommentDto
        {
            Body = updated.Body,
            UserId = updated.UserId,
            PostId = updated.PostId,
            id = updated.Id,
        });
    }

    // DELETE endpoint to delete a user by username
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        if (comment is null)
        {
            return NotFound("Post not found");
        }

        await commentRepository.DeleteAsync(id);
        commentCache.Remove(id);

        return NoContent();
    }

    //Alle get For the GetMany users endpoint, it must be possible to-
    //filter the users by whether the username contains a certain string.
    //You may add more filter criteria.
    [HttpGet]
    public ActionResult<IEnumerable<User>> getAllComments([FromQuery] int? postId = null)
    {
        {
            var comments = commentRepository.GetMany();

            // Hvis der er angivet et id
            if (postId.HasValue)
            {
                comments = comments.Where(c => c.PostId == postId.Value);
            }

            var commentDto = comments.Select(c => new CommentDto
            {
                Body = c.Body,
                UserId = c.UserId,
                PostId = c.PostId,
                id = c.Id,
            });

            return Ok(commentDto);
        }
    }
}