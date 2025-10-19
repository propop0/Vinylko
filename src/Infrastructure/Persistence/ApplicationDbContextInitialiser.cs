using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Equipments;
using Domain.MaintenanceSchedules;
using Domain.WorkOrders;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _dbContext.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!await _dbContext.Equipments.AnyAsync())
            {
                var e1 = Equipment.New(Guid.NewGuid(), "Industrial Lathe", "LT-2000", "SN-LT-2000-001", "Workshop A", DateTime.UtcNow.AddYears(-3));
                var e2 = Equipment.New(Guid.NewGuid(), "CNC Machine", "CNC-500X", "SN-CNC-500X-001", "Production Floor", DateTime.UtcNow.AddYears(-2));
                var e3 = Equipment.New(Guid.NewGuid(), "Hydraulic Press", "HP-1000", "SN-HP-1000-001", "Workshop B", DateTime.UtcNow.AddYears(-1));

                await _dbContext.Equipments.AddRangeAsync(e1, e2, e3);
                await _dbContext.SaveChangesAsync();

                // Maintenance schedules: monthly inspection for each, plus quarterly and annual examples
                var ms1 = MaintenanceSchedule.New(Guid.NewGuid(), e1.Id, "Monthly inspection", "Monthly basic inspection", MaintenanceFrequency.Monthly, DateTime.UtcNow.AddDays(30));
                var ms2 = MaintenanceSchedule.New(Guid.NewGuid(), e2.Id, "Monthly inspection", "Monthly basic inspection", MaintenanceFrequency.Monthly, DateTime.UtcNow.AddDays(30));
                var ms3 = MaintenanceSchedule.New(Guid.NewGuid(), e3.Id, "Monthly inspection", "Monthly basic inspection", MaintenanceFrequency.Monthly, DateTime.UtcNow.AddDays(30));
                var ms4 = MaintenanceSchedule.New(Guid.NewGuid(), e1.Id, "Quarterly deep cleaning", "Deep cleaning every quarter", MaintenanceFrequency.Quarterly, DateTime.UtcNow.AddDays(90));
                var ms5 = MaintenanceSchedule.New(Guid.NewGuid(), e2.Id, "Annual calibration", "Yearly calibration", MaintenanceFrequency.Annually, DateTime.UtcNow.AddMonths(12));

                await _dbContext.MaintenanceSchedules.AddRangeAsync(ms1, ms2, ms3, ms4, ms5);
                await _dbContext.SaveChangesAsync();

                // Work orders sample
                var wo1 = WorkOrder.New(Guid.NewGuid(), $"WO-{DateTime.UtcNow:yyyyMMddHHmmss}-A1", e1.Id, "Replace belt", "Replace worn belt", WorkOrderPriority.Medium, DateTime.UtcNow.AddDays(3));
                var wo2 = WorkOrder.New(Guid.NewGuid(), $"WO-{DateTime.UtcNow:yyyyMMddHHmmss}-B2", e2.Id, "Emergency stop fault", "Investigate emergency stop not working", WorkOrderPriority.Critical, DateTime.UtcNow.AddDays(1));
                var wo3 = WorkOrder.New(Guid.NewGuid(), $"WO-{DateTime.UtcNow:yyyyMMddHHmmss}-C3", e3.Id, "Lubrication", "Lubricate bearings", WorkOrderPriority.Low, DateTime.UtcNow.AddDays(7));
                wo2.StartWork();
                wo3.StartWork();
                wo3.Complete("Lubricated and tested");

                await _dbContext.WorkOrders.AddRangeAsync(wo1, wo2, wo3);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
