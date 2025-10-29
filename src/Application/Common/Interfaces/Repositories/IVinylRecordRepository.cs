using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.VinylRecords;

namespace Application.Common.Interfaces.Repositories;

public interface IVinylRecordRepository
{
    Task<VinylRecord> AddAsync(VinylRecord entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecord>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken);
    Task<VinylRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(VinylRecord entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasSalesAsync(Guid vinylRecordId, CancellationToken cancellationToken);
}


