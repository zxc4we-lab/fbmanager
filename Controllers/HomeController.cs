using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FacebookManager.Controllers;

/// <summary>
/// Provides high level home and error pages.  The home page redirects
/// authenticated users to the dashboard and unauthenticated users to
/// a welcome page.
/// </summary>
public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            // Redirect signed in users directly to the dashboard.
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}