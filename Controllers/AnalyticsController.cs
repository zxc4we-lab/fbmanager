using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;

namespace FacebookManager.Controllers;

/// <summary>
/// Displays aggregate analytics across pages and posts.  The analytics page
/// provides a simple summary table and can be expanded to include graphs.
/// </summary>
[Authorize]
public class AnalyticsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Compute aggregate sums across all analytics records.
        var totalReach = await _context.AnalyticsData.SumAsync(a => (int?)a.Reach) ?? 0;
        var totalReactions = await _context.AnalyticsData.SumAsync(a => (int?)a.Reactions) ?? 0;
        var totalComments = await _context.AnalyticsData.SumAsync(a => (int?)a.Comments) ?? 0;
        var totalShares = await _context.AnalyticsData.SumAsync(a => (int?)a.Shares) ?? 0;
        ViewData["TotalReach"] = totalReach;
        ViewData["TotalReactions"] = totalReactions;
        ViewData["TotalComments"] = totalComments;
        ViewData["TotalShares"] = totalShares;
        return View();
    }
}