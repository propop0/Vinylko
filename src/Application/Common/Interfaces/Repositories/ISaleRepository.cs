using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Sales;

namespace Application.Common.Interfaces.Repositories;

public interface ISaleRepository
{
    Task<Sale> AddAsync(Sale entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetByRecordIdAsync(Guid recordId, CancellationToken cancellationToken);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Sale entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}


