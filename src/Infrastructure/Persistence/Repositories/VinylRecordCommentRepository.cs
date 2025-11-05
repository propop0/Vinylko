using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VinylRecordCommentRepository : IVinylRecordCommentRepository, IVinylRecordCommentQueries
{
    private readonly ApplicationDbContext _context;

    public VinylRecordCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VinylRecordComment> AddAsync(VinylRecordComment entity, CancellationToken cancellationToken)
    {
        await _context.VinylRecordComments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(VinylRecordComment entity, CancellationToken cancellationToken)
    {
        _context.VinylRecordComments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.VinylRecordComments.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.VinylRecordComments.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<VinylRecordComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.VinylRecordComments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecordComment>> GetByVinylRecordIdAsync(Guid vinylRecordId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecordComments
            .AsNoTracking()
            .Where(c => c.VinylRecordId == vinylRecordId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}


