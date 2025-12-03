using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class RecordReleaseTypeRepository : IRecordReleaseTypeRepository
{
    private readonly ApplicationDbContext _context;

    public RecordReleaseTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecordReleaseType> AddAsync(RecordReleaseType entity, CancellationToken cancellationToken)
    {
        await _context.RecordReleaseTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<RecordReleaseType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.RecordReleaseTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<RecordReleaseType>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var recordReleaseType = await _context.RecordReleaseTypes
            .FirstOrDefaultAsync(rrt => rrt.Id == id, cancellationToken);
        return recordReleaseType == null ? Option.None<RecordReleaseType>() : Option.Some(recordReleaseType);
    }

    public async Task<Option<RecordReleaseType>> GetByVinylRecordIdAsync(Guid vinylRecordId, CancellationToken cancellationToken)
    {
        var recordReleaseType = await _context.RecordReleaseTypes
            .FirstOrDefaultAsync(rrt => rrt.VinylRecordId == vinylRecordId, cancellationToken);
        return recordReleaseType == null ? Option.None<RecordReleaseType>() : Option.Some(recordReleaseType);
    }

    public async Task UpdateAsync(RecordReleaseType entity, CancellationToken cancellationToken)
    {
        _context.RecordReleaseTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _context.RecordReleaseTypes.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.RecordReleaseTypes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

