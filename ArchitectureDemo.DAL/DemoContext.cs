using Microsoft.EntityFrameworkCore;
using ArchitectureDemo.DAL.Entities;

#pragma warning disable CS8618

namespace ArchitectureDemo.DAL;

internal sealed class DemoContext : DbContext
{
    public DemoContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<User> Users { get; set; }

    public DbSet<UserFile> UserFiles { get; set; }
}
