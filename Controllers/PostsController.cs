using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager.Controllers;

/// <summary>
/// Manages CRUD operations for published posts.  Users can view a list of
/// posts, create new posts, edit existing posts and delete posts.  Posts
/// belong to pages; a SelectList of pages is passed to the views to
/// facilitate selection.
/// </summary>
[Authorize]
public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Posts
    public async Task<IActionResult> Index()
    {
        var posts = await _context.Posts.Include(p => p.Page).ToListAsync();
        return View(posts);
    }

    // GET: Posts/Create
    public IActionResult Create()
    {
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name");
        return View();
    }

    // POST: Posts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
        if (ModelState.IsValid)
        {
            post.CreatedAt = DateTime.UtcNow;
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name", post.PageId);
        return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        ViewBag.PageId = new SelectList(_context.Pages, "Id", "Name", post.PageId);
        return View(post);
    }

    // POST: Posts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Post post)
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
                if (!_context.Posts.Any(e => e.Id == post.Id))
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

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = await _context.Posts
            .Include(p => p.Page)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}