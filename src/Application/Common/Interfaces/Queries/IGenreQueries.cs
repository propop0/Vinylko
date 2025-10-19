using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Genres;

namespace Application.Common.Interfaces.Queries;

public interface IGenreQueries
{
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken);
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}


