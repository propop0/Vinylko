using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Artists;

namespace Application.Common.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Artist> AddAsync(Artist entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken cancellationToken);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Artist entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasVinylRecordsAsync(Guid artistId, CancellationToken cancellationToken);
}


