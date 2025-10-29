using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VinylRecordRepository : IVinylRecordRepository, IVinylRecordQueries
{
    private readonly ApplicationDbContext _context;

    public VinylRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VinylRecord> AddAsync(VinylRecord entity, CancellationToken cancellationToken)
    {
        await _context.VinylRecords.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<VinylRecord>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.VinylRecords.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.ArtistId == artistId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.Genre == genreId.ToString())
            .ToListAsync(cancellationToken);
    }

    public async Task<VinylRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords.FirstOrDefaultAsync(vr => vr.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(VinylRecord entity, CancellationToken cancellationToken)
    {
        _context.VinylRecords.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasSales = await _context.Sales.AnyAsync(s => s.RecordId == id, cancellationToken);
        if (hasSales)
        {
            throw new InvalidOperationException("Cannot delete vinyl record. There are sales associated with this record.");
        }

        var existing = await _context.VinylRecords.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.VinylRecords.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByStatusAsync(VinylRecordStatus status, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.Title.Contains(title))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByReleaseYearAsync(int year, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.ReleaseYear == year)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VinylRecord>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords
            .AsNoTracking()
            .Where(vr => vr.Price >= minPrice && vr.Price <= maxPrice)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTitleAndArtistAsync(string title, Guid artistId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords.AnyAsync(vr => vr.Title == title && vr.ArtistId == artistId, cancellationToken);
    }

    public async Task<bool> HasSalesAsync(Guid vinylRecordId, CancellationToken cancellationToken)
    {
        return await _context.Sales.AnyAsync(s => s.RecordId == vinylRecordId, cancellationToken);
    }
}
