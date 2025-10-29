using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Artists;

namespace Application.Common.Interfaces.Queries;

public interface IArtistQueries
{
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken cancellationToken);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Artist>> GetByCountryAsync(string country, CancellationToken cancellationToken);
}


