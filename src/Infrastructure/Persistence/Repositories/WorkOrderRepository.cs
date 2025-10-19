using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WorkOrderRepository : IWorkOrderRepository, IWorkOrderQueries
{
    private readonly ApplicationDbContext _context;

    public WorkOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder> AddAsync(WorkOrder entity, CancellationToken cancellationToken)
    {
        await _context.WorkOrders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<WorkOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.WorkOrders.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.WorkOrders.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(WorkOrder entity, CancellationToken cancellationToken)
    {
        _context.WorkOrders.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.WorkOrders.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.WorkOrders.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsByWorkOrderNumberAsync(string workOrderNumber, CancellationToken cancellationToken)
    {
        return await _context.WorkOrders.AnyAsync(w => w.WorkOrderNumber == workOrderNumber, cancellationToken);
    }
}