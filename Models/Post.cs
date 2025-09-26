using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookManager.Models;

/// <summary>
/// Represents a published Facebook post.  Posts are created by a user
/// against a specific page.  A post may have associated analytics data
/// recorded after publication.
/// </summary>
public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Optional image URL associated with the post.  In a real system this
    /// would be stored in blob storage and referenced here.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Foreign key linking the post to a Page entity.
    /// </summary>
    public int PageId { get; set; }

    [ForeignKey(nameof(PageId))]
    public Page? Page { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for analytics.  A post may have zero or more
    /// AnalyticsData entries depending on the tracking granularity.
    /// </summary>
    public ICollection<AnalyticsData> Analytics { get; set; } = new List<AnalyticsData>();
}