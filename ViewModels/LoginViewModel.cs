using System.ComponentModel.DataAnnotations;

namespace FacebookManager.ViewModels;

/// <summary>
/// View model used for the login page.  Includes basic validation
/// attributes for required fields.
/// </summary>
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}