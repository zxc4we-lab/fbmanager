using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers.Api;

/// <summary>
/// Provides JSON endpoints for analytics data.  Users can query aggregated
/// metrics or individual analytics records.  Only authenticated users
/// have access.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnalyticsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnalyticsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Analytics
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnalyticsData>>> GetAnalytics()
    {
        return await _context.AnalyticsData.ToListAsync();
    }

    // GET: api/Analytics/aggregate
    [HttpGet("aggregate")]
    public async Task<ActionResult<object>> GetAggregated()
    {
        var totalReach = await _context.AnalyticsData.SumAsync(a => (int?)a.Reach) ?? 0;
        var totalReactions = await _context.AnalyticsData.SumAsync(a => (int?)a.Reactions) ?? 0;
        var totalComments = await _context.AnalyticsData.SumAsync(a => (int?)a.Comments) ?? 0;
        var totalShares = await _context.AnalyticsData.SumAsync(a => (int?)a.Shares) ?? 0;
        return new { totalReach, totalReactions, totalComments, totalShares };
    }

    // GET: api/Analytics/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AnalyticsData>> GetAnalyticsData(int id)
    {
        var data = await _context.AnalyticsData.FindAsync(id);
        if (data == null)
        {
            return NotFound();
        }
        return data;
    }

    // POST: api/Analytics
    [HttpPost]
    public async Task<ActionResult<AnalyticsData>> CreateAnalytics(AnalyticsData data)
    {
        _context.AnalyticsData.Add(data);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAnalyticsData), new { id = data.Id }, data);
    }

    // PUT: api/Analytics/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnalytics(int id, AnalyticsData data)
    {
        if (id != data.Id)
        {
            return BadRequest();
        }
        _context.Entry(data).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.AnalyticsData.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Analytics/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnalytics(int id)
    {
        var data = await _context.AnalyticsData.FindAsync(id);
        if (data == null)
        {
            return NotFound();
        }
        _context.AnalyticsData.Remove(data);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}