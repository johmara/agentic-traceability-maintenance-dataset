using Microsoft.EntityFrameworkCore;
using ReferenceManager.Models;

namespace ReferenceManager.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Paper> Papers => Set<Paper>();
    public DbSet<Group> Groups => Set<Group>(); // &line[Groups]
    public DbSet<Author> Authors => Set<Author>(); // &line[Authors]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // &begin[Authors]
        modelBuilder.Entity<Author>()
            .OwnsMany(a => a.Affiliations, b => b.ToJson());

        modelBuilder.Entity<Paper>()
            .HasMany(p => p.Authors)
            .WithMany(a => a.Papers)
            .UsingEntity("PaperAuthor");
        // &end[Authors]

        // &begin[Groups]
        modelBuilder.Entity<Group>()
            .HasMany(g => g.Papers)
            .WithMany(p => p.Groups)
            .UsingEntity("PaperGroup");
        // &end[Groups]
    }
}
