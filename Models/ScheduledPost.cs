using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookManager.Models;

/// <summary>
/// Represents a post that is scheduled to be published at a later date.
/// Scheduled posts are stored separately from published posts so that
/// publication can be triggered by a background job or manual action.
/// </summary>
public class ScheduledPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    /// <summary>
    /// Time the post should be published.  Should be in UTC to avoid
    /// timezone confusion.
    /// </summary>
    public DateTime ScheduledFor { get; set; }

    /// <summary>
    /// Indicates whether the post has been published.  In a real system
    /// scheduled posts would be processed by a background worker that
    /// transfers them to the Posts table when due.
    /// </summary>
    public bool IsPublished { get; set; }

    public int PageId { get; set; }
    [ForeignKey(nameof(PageId))]
    public Page? Page { get; set; }
}