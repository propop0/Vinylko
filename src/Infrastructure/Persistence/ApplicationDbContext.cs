using System.Reflection;
using Domain.Equipments;
using Domain.MaintenanceSchedules;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipments { get; init; } = null!;
    public DbSet<MaintenanceSchedule> MaintenanceSchedules { get; init; } = null!;
    public DbSet<WorkOrder> WorkOrders { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}