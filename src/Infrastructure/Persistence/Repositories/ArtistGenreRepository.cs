using Application.Common.Interfaces.Repositories;
using Domain.ArtistGenres;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ArtistGenreRepository : IArtistGenreRepository
{
    private readonly ApplicationDbContext _context;

    public ArtistGenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArtistGenre> AddAsync(ArtistGenre entity, CancellationToken cancellationToken)
    {
        await _context.ArtistGenres.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<ArtistGenre>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken)
    {
        return await _context.ArtistGenres
            .AsNoTracking()
            .Where(ag => ag.ArtistId == artistId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ArtistGenre>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken)
    {
        return await _context.ArtistGenres
            .AsNoTracking()
            .Where(ag => ag.GenreId == genreId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ArtistGenre?> GetByArtistAndGenreAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken)
    {
        return await _context.ArtistGenres
            .FirstOrDefaultAsync(ag => ag.ArtistId == artistId && ag.GenreId == genreId, cancellationToken);
    }

    public async Task DeleteAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken)
    {
        var existing = await _context.ArtistGenres
            .FirstOrDefaultAsync(ag => ag.ArtistId == artistId && ag.GenreId == genreId, cancellationToken);
        
        if (existing != null)
        {
            _context.ArtistGenres.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken)
    {
        return await _context.ArtistGenres
            .AnyAsync(ag => ag.ArtistId == artistId && ag.GenreId == genreId, cancellationToken);
    }
}

