using Microsoft.EntityFrameworkCore;

namespace AnkarefApp.Data;

public class AllDbContext : DbContext
{
    public AllDbContext(DbContextOptions<AllDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ActivityCategory> ActivityCategories { get; set; }
    public DbSet<ActivityTable> Activities { get; set; }
    public DbSet<ActivityParticipant> ActivityParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityTable>()
            .HasOne(a => a.CreatingUser)
            .WithMany()
            .HasForeignKey(a => a.CreatingUserId);

        modelBuilder.Entity<ActivityTable>()
            .HasOne(a => a.ActivityCategory)
            .WithMany()
            .HasForeignKey(a => a.ActivityCategoryId);

        modelBuilder.Entity<ActivityParticipant>()
            .HasKey(ap => new { ap.ActivityId, ap.UserId });

        modelBuilder.Entity<ActivityParticipant>()
            .HasOne(ap => ap.ActivityTable)
            .WithMany()
            .HasForeignKey(ap => ap.ActivityId);

        modelBuilder.Entity<ActivityParticipant>()
            .HasOne(ap => ap.User)
            .WithMany()
            .HasForeignKey(ap => ap.UserId);
    }
}