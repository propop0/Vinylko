using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Artists;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Artist> AddAsync(Artist entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<Artist>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Artist entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> HasVinylRecordsAsync(Guid artistId, CancellationToken cancellationToken);
}


