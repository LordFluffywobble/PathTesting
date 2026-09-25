using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Testing.PathFinder;

public class PathFinderDbContext : DbContext
{
    public PathFinderDbContext(DbContextOptions<PathFinderDbContext> options) : base(options) { }

    // Dette oppretter tabellen i PostgreSQL-databasen din
    public DbSet<Coordinates> Coordinates => Set<Coordinates>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coordinates>().ToTable("coordinates");
    }
}