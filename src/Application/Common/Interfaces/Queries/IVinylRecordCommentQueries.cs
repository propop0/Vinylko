using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.VinylRecords;

namespace Application.Common.Interfaces.Queries;

public interface IVinylRecordCommentQueries
{
    Task<IReadOnlyList<VinylRecordComment>> GetByVinylRecordIdAsync(Guid vinylRecordId, CancellationToken cancellationToken);
    Task<VinylRecordComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}


