using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.RecordReleaseTypes.Queries;

public record GetRecordReleaseTypesQuery : IRequest<IReadOnlyList<RecordReleaseType>>;

public class GetRecordReleaseTypesQueryHandler : IRequestHandler<GetRecordReleaseTypesQuery, IReadOnlyList<RecordReleaseType>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;

    public GetRecordReleaseTypesQueryHandler(IRecordReleaseTypeRepository recordReleaseTypeRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
    }

    public async Task<IReadOnlyList<RecordReleaseType>> Handle(GetRecordReleaseTypesQuery request, CancellationToken cancellationToken)
    {
        return await _recordReleaseTypeRepository.GetAllAsync(cancellationToken);
    }
}

