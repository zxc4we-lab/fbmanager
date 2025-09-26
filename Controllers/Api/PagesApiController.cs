using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers.Api;

/// <summary>
/// Provides JSON endpoints for pages management.  Secure endpoints that
/// allow listing, creating, updating and deleting pages via REST.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PagesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PagesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Pages
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Page>>> GetPages()
    {
        return await _context.Pages.Include(p => p.Posts).Include(p => p.ScheduledPosts).ToListAsync();
    }

    // GET: api/Pages/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Page>> GetPage(int id)
    {
        var page = await _context.Pages.Include(p => p.Posts).Include(p => p.ScheduledPosts).FirstOrDefaultAsync(p => p.Id == id);
        if (page == null)
        {
            return NotFound();
        }
        return page;
    }

    // POST: api/Pages
    [HttpPost]
    public async Task<ActionResult<Page>> CreatePage(Page page)
    {
        _context.Pages.Add(page);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
    }

    // PUT: api/Pages/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePage(int id, Page page)
    {
        if (id != page.Id)
        {
            return BadRequest();
        }
        _context.Entry(page).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Pages.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Pages/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page == null)
        {
            return NotFound();
        }
        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}