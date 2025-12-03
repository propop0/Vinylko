using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;
using Optional;

namespace Application.RecordReleaseTypes.Queries;

public record GetRecordReleaseTypeByIdQuery : IRequest<Option<RecordReleaseType>>
{
    public required Guid Id { get; init; }
}

public class GetRecordReleaseTypeByIdQueryHandler : IRequestHandler<GetRecordReleaseTypeByIdQuery, Option<RecordReleaseType>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;

    public GetRecordReleaseTypeByIdQueryHandler(IRecordReleaseTypeRepository recordReleaseTypeRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
    }

    public async Task<Option<RecordReleaseType>> Handle(GetRecordReleaseTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _recordReleaseTypeRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
