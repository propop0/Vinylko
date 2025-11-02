using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Artists;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ArtistRepository : IArtistRepository, IArtistQueries
{
    private readonly ApplicationDbContext _context;

    public ArtistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Artist> AddAsync(Artist entity, CancellationToken cancellationToken)
    {
        await _context.Artists.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Artists.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Artists.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Artist entity, CancellationToken cancellationToken)
    {
        _context.Artists.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasVinylRecords = await _context.VinylRecords.AnyAsync(vr => vr.ArtistId == id, cancellationToken);
        if (hasVinylRecords)
        {
            throw new InvalidOperationException("Cannot delete artist. There are vinyl records associated with this artist.");
        }

        var existing = await _context.Artists.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.Artists.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Artists.AnyAsync(a => a.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Artist>> GetByCountryAsync(string country, CancellationToken cancellationToken)
    {
        return await _context.Artists
            .AsNoTracking()
            .Where(a => a.Country == country)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasVinylRecordsAsync(Guid artistId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords.AnyAsync(vr => vr.ArtistId == artistId, cancellationToken);
    }
}
