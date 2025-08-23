using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data.Auth;
using MovieRecommendation.Data.Movie;
using MovieRecommendation.Data.Review;

namespace MovieRecommendation.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserData> Users { get; set; }
    public DbSet<MovieData> Movies { get; set; }
    public DbSet<CategoryData> Categories { get; set; }
    public DbSet<ReviewData> Reviews { get; set; }

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
