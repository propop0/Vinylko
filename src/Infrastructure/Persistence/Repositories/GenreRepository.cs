using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Genres;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GenreRepository : IGenreRepository, IGenreQueries
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Genre> AddAsync(Genre entity, CancellationToken cancellationToken)
    {
        await _context.Genres.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Genres.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Genre entity, CancellationToken cancellationToken)
    {
        _context.Genres.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {

        var hasVinylRecords = await _context.VinylRecords.AnyAsync(vr => vr.Genre == id.ToString(), cancellationToken);
        if (hasVinylRecords)
        {
            throw new InvalidOperationException("Cannot delete genre. There are vinyl records associated with this genre.");
        }

        var existing = await _context.Genres.FindAsync(new object[] { id }, cancellationToken);
        if (existing != null)
        {
            _context.Genres.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Genres.AnyAsync(g => g.Name == name, cancellationToken);
    }

    public async Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
    }

    public async Task<bool> HasVinylRecordsAsync(Guid genreId, CancellationToken cancellationToken)
    {
        return await _context.VinylRecords.AnyAsync(vr => vr.Genre == genreId.ToString(), cancellationToken);
    }
}
