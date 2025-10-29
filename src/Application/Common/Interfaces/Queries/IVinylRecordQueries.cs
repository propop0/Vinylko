using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.VinylRecords;

namespace Application.Common.Interfaces.Queries;

public interface IVinylRecordQueries
{
    Task<IReadOnlyList<VinylRecord>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken);
    Task<VinylRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetByStatusAsync(VinylRecordStatus status, CancellationToken cancellationToken);
}


