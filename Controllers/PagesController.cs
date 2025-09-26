using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers;

/// <summary>
/// Manages Facebook pages.  Users can create, edit and delete pages.
/// Pages serve as containers for posts and scheduled posts.
/// </summary>
[Authorize]
public class PagesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Pages
    public async Task<IActionResult> Index()
    {
        var pages = await _context.Pages.Include(p => p.Posts).Include(p => p.ScheduledPosts).ToListAsync();
        return View(pages);
    }

    // GET: Pages/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Pages/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Page page)
    {
        if (ModelState.IsValid)
        {
            _context.Add(page);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(page);
    }

    // GET: Pages/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var page = await _context.Pages.FindAsync(id);
        if (page == null)
        {
            return NotFound();
        }
        return View(page);
    }

    // POST: Pages/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Page page)
    {
        if (id != page.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(page);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pages.Any(e => e.Id == page.Id))
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
        return View(page);
    }

    // GET: Pages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var page = await _context.Pages
            .Include(p => p.Posts)
            .Include(p => p.ScheduledPosts)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (page == null)
        {
            return NotFound();
        }
        return View(page);
    }

    // POST: Pages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page != null)
        {
            _context.Pages.Remove(page);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}