using Microsoft.AspNetCore.Identity;

namespace FacebookManager.Models;

/// <summary>
/// ApplicationUser extends IdentityUser to allow for future expansion of the
/// user profile.  Additional properties (e.g. display name, avatar URL)
/// can be added here without altering the Identity schema directly.
/// </summary>
public class ApplicationUser : IdentityUser
{
    // Additional user properties can be added here as needed.
}