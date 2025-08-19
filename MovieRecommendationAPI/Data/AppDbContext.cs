using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data.Auth;

namespace MovieRecommendation.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserData> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserData>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
