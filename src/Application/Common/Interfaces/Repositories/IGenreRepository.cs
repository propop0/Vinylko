using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Genres;

namespace Application.Common.Interfaces.Repositories;

public interface IGenreRepository
{
    Task<Genre> AddAsync(Genre entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken);
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Genre entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasVinylRecordsAsync(Guid genreId, CancellationToken cancellationToken);
}


