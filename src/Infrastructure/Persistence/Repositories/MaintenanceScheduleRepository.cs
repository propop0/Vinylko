using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.MaintenanceSchedules;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MaintenanceScheduleRepository : IMaintenanceScheduleRepository, IMaintenanceScheduleQueries
{
    private readonly ApplicationDbContext _context;

    public MaintenanceScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceSchedule> AddAsync(MaintenanceSchedule entity, CancellationToken cancellationToken)
    {
        await _context.MaintenanceSchedules.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<MaintenanceSchedule>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.MaintenanceSchedules.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MaintenanceSchedule>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken)
    {
        return await _context.MaintenanceSchedules
            .AsNoTracking()
            .Where(s => s.EquipmentId == equipmentId && s.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<MaintenanceSchedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.MaintenanceSchedules.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(MaintenanceSchedule entity, CancellationToken cancellationToken)
    {
        _context.MaintenanceSchedules.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.MaintenanceSchedules.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            existing.Deactivate();
            _context.MaintenanceSchedules.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.MaintenanceSchedules.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.MaintenanceSchedules.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
