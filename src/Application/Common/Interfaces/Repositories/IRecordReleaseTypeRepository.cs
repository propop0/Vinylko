using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.VinylRecords;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IRecordReleaseTypeRepository
{
    Task<RecordReleaseType> AddAsync(RecordReleaseType entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<RecordReleaseType>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<RecordReleaseType>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Option<RecordReleaseType>> GetByVinylRecordIdAsync(Guid vinylRecordId, CancellationToken cancellationToken);
    Task UpdateAsync(RecordReleaseType entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}

