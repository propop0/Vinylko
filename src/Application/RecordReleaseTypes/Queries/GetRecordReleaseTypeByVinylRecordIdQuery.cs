using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;
using Optional;

namespace Application.RecordReleaseTypes.Queries;

public record GetRecordReleaseTypeByVinylRecordIdQuery : IRequest<Option<RecordReleaseType>>
{
    public required Guid VinylRecordId { get; init; }
}

public class GetRecordReleaseTypeByVinylRecordIdQueryHandler : IRequestHandler<GetRecordReleaseTypeByVinylRecordIdQuery, Option<RecordReleaseType>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;

    public GetRecordReleaseTypeByVinylRecordIdQueryHandler(IRecordReleaseTypeRepository recordReleaseTypeRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
    }

    public async Task<Option<RecordReleaseType>> Handle(GetRecordReleaseTypeByVinylRecordIdQuery request, CancellationToken cancellationToken)
    {
        return await _recordReleaseTypeRepository.GetByVinylRecordIdAsync(request.VinylRecordId, cancellationToken);
    }
}
