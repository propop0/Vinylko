using System.Reflection;
using Domain.Artists;
using Domain.Genres;
using Domain.Sales;
using Domain.VinylRecords;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Artist> Artists { get; init; } = null!;
    public DbSet<Genre> Genres { get; init; } = null!;
    public DbSet<VinylRecord> VinylRecords { get; init; } = null!;
    public DbSet<Sale> Sales { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}