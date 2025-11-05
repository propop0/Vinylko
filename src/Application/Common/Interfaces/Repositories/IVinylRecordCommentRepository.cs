using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.VinylRecords;

namespace Application.Common.Interfaces.Repositories;

public interface IVinylRecordCommentRepository
{
    Task<VinylRecordComment> AddAsync(VinylRecordComment entity, CancellationToken cancellationToken);
    Task UpdateAsync(VinylRecordComment entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<VinylRecordComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<VinylRecordComment>> GetByVinylRecordIdAsync(Guid vinylRecordId, CancellationToken cancellationToken);
}


