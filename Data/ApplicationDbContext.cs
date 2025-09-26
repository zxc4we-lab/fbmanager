using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Models;

namespace FacebookManager.Data;

/// <summary>
/// ApplicationDbContext defines the Entity Framework model for the
/// application.  Inherits from IdentityDbContext to integrate ASP.NET
/// Identity tables alongside application specific entities.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Represents Facebook posts created in the management tool.
    /// </summary>
    public DbSet<Post> Posts { get; set; } = default!;

    /// <summary>
    /// Represents scheduled posts that will be published at a future time.
    /// </summary>
    public DbSet<ScheduledPost> ScheduledPosts { get; set; } = default!;

    /// <summary>
    /// Represents Facebook pages that the user manages.
    /// </summary>
    public DbSet<Page> Pages { get; set; } = default!;

    /// <summary>
    /// Represents basic analytic metrics for posts and pages.
    /// </summary>
    public DbSet<AnalyticsData> AnalyticsData { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Configure any custom relationships or table names here.  By
        // default EF will infer keys and relationships based on naming
        // conventions.
    }
}