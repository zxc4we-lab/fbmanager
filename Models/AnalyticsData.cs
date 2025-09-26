using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookManager.Models;

/// <summary>
/// Simple analytics record capturing basic engagement statistics for
/// either a post or an entire page.  In a real application you would
/// likely have more granular metrics and separate tables for posts and
/// pages.
/// </summary>
public class AnalyticsData
{
    [Key]
    public int Id { get; set; }

    public int? PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post? Post { get; set; }

    public int? PageId { get; set; }
    [ForeignKey(nameof(PageId))]
    public Page? Page { get; set; }

    /// <summary>
    /// Total reach (impressions) for the post or page.
    /// </summary>
    public int Reach { get; set; }

    /// <summary>
    /// Number of reactions (likes, loves, etc.).
    /// </summary>
    public int Reactions { get; set; }

    /// <summary>
    /// Number of comments.
    /// </summary>
    public int Comments { get; set; }

    /// <summary>
    /// Number of shares.
    /// </summary>
    public int Shares { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}