using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Artists;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IArtistQueries
{
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<Artist>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Artist>> GetByCountryAsync(string country, CancellationToken cancellationToken);
}


