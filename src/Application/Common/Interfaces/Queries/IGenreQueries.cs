using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Genres;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IGenreQueries
{
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<Genre>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Option<Genre>> GetByNameAsync(string name, CancellationToken cancellationToken);
}


