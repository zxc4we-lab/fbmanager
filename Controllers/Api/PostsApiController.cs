using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers.Api;

/// <summary>
/// Provides JSON endpoints for creating, reading, updating and deleting
/// posts.  All endpoints require authentication and return standard
/// HTTP status codes.  While the web UI interacts with these controllers
/// via forms, external tools or scripts could call these APIs directly.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PostsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
    {
        return await _context.Posts.Include(p => p.Page).ToListAsync();
    }

    // GET: api/Posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _context.Posts.Include(p => p.Page).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return post;
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    // PUT: api/Posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, Post post)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }
        _context.Entry(post).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Posts.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}