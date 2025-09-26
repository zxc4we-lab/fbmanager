using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;

namespace FacebookManager.Controllers;

/// <summary>
/// Provides the main dashboard view summarising key metrics for the
/// logged in user.  The dashboard displays counts of pages, posts and
/// scheduled posts as well as aggregate analytics data.
/// </summary>
[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var pageCount = await _context.Pages.CountAsync();
        var postCount = await _context.Posts.CountAsync();
        var scheduledCount = await _context.ScheduledPosts.CountAsync();
        var analyticsCount = await _context.AnalyticsData.CountAsync();

        // Provide metrics to the view via ViewData.  In larger applications
        // you might create a dedicated view model instead.
        ViewData["PageCount"] = pageCount;
        ViewData["PostCount"] = postCount;
        ViewData["ScheduledCount"] = scheduledCount;
        ViewData["AnalyticsCount"] = analyticsCount;
        return View();
    }
}