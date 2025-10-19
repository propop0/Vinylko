using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentRepository : IEquipmentRepository, IEquipmentQueries
{
    private readonly ApplicationDbContext _context;

    public EquipmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Equipment> AddAsync(Equipment entity, CancellationToken cancellationToken)
    {
        await _context.Equipments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Equipments.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Equipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Equipments.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Equipment entity, CancellationToken cancellationToken)
    {
        _context.Equipments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.Equipments.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.Equipments.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken)
    {
        return await _context.Equipments.AnyAsync(e => e.SerialNumber == serialNumber, cancellationToken);
    }
}