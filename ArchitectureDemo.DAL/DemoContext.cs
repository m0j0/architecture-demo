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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(b => b.Email)
            .IsUnique()
            .HasDatabaseName(User.EmailUniqueIndexName);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Parent)
            .WithMany(u => u.Children)
            .HasForeignKey(u => u.ParentId)
            .HasConstraintName(User.ParentIdForeignKeyName)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<UserFile>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Files)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    internal static void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder,
        string connectionString)
    {
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();

#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }
}
