using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SaleRepository : ISaleRepository, ISaleQueries
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sale> AddAsync(Sale entity, CancellationToken cancellationToken)
    {
        await _context.Sales.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Sales.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByRecordIdAsync(Guid recordId, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .AsNoTracking()
            .Where(s => s.RecordId == recordId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Sales.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Sale entity, CancellationToken cancellationToken)
    {
        _context.Sales.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.Sales.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<Sale>> GetByStatusAsync(SaleStatus status, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .AsNoTracking()
            .Where(s => s.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByCustomerEmailAsync(string customerEmail, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .AsNoTracking()
            .Where(s => s.CustomerEmail == customerEmail)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .AsNoTracking()
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken)
    {
        return await _context.Sales.AnyAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    public async Task<decimal> GetTotalSalesAmountAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales
            .AsNoTracking()
            .Where(s => s.Status == SaleStatus.Completed);

        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        return await query.SumAsync(s => s.Price, cancellationToken);
    }
}
