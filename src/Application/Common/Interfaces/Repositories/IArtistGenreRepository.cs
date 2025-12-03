using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.ArtistGenres;

namespace Application.Common.Interfaces.Repositories;

public interface IArtistGenreRepository
{
    Task<ArtistGenre> AddAsync(ArtistGenre entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<ArtistGenre>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ArtistGenre>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken);
    Task<ArtistGenre?> GetByArtistAndGenreAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid artistId, Guid genreId, CancellationToken cancellationToken);
}

