using System.ComponentModel.DataAnnotations;

namespace FacebookManager.Models;

/// <summary>
/// Represents a Facebook page managed by the user.  Pages hold posts and
/// scheduled posts, and basic metadata such as the page name and description.
/// </summary>
public class Page
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<ScheduledPost> ScheduledPosts { get; set; } = new List<ScheduledPost>();
}