using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookManager.Controllers;

/// <summary>
/// Settings controller allows the user to configure application preferences
/// such as notification settings or connection information.  This simple
/// implementation provides a placeholder view.
/// </summary>
[Authorize]
public class SettingsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}