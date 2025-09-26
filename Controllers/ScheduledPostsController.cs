using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers;

/// <summary>
/// Manages scheduled posts.  Users can schedule posts to be published at
/// a future date and time.  Although this implementation does not include
/// a background worker to automatically publish posts when due, it lays
/// the groundwork for that feature by tracking the schedule and an
/// IsPublished flag.
/// </summary>
[Authorize]
public class ScheduledPostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ScheduledPostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ScheduledPosts
    public async Task<IActionResult> Index()
    {
        var scheduled = await _context.ScheduledPosts.Include(s => s.Page).ToListAsync();
        return View(scheduled);
    }

    // GET: ScheduledPosts/Create
    public IActionResult Create()
    {
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name");
        return View();
    }

    // POST: ScheduledPosts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduledPost post)
    {
        if (ModelState.IsValid)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name", post.PageId);
        return View(post);
    }

    // GET: ScheduledPosts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = await _context.ScheduledPosts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name", post.PageId);
        return View(post);
    }

    // POST: ScheduledPosts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ScheduledPost post)
    {
        if (id != post.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ScheduledPosts.Any(e => e.Id == post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name", post.PageId);
        return View(post);
    }

    // GET: ScheduledPosts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = await _context.ScheduledPosts.Include(s => s.Page)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    // POST: ScheduledPosts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.ScheduledPosts.FindAsync(id);
        if (post != null)
        {
            _context.ScheduledPosts.Remove(post);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Manual publish action moves a scheduled post into the published posts
    /// table.  In a real deployment this would be performed by a background
    /// service when the scheduled time is reached.
    /// </summary>
    public async Task<IActionResult> Publish(int id)
    {
        var scheduled = await _context.ScheduledPosts.FindAsync(id);
        if (scheduled == null)
        {
            return NotFound();
        }
        // Only publish if not already done
        if (!scheduled.IsPublished)
        {
            var post = new Post
            {
                Title = scheduled.Title,
                Content = scheduled.Content,
                ImageUrl = scheduled.ImageUrl,
                PageId = scheduled.PageId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Posts.Add(post);
            scheduled.IsPublished = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}