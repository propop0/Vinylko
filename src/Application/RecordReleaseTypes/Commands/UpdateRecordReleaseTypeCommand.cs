using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.RecordReleaseTypes.Commands;

public record UpdateRecordReleaseTypeCommand : IRequest<ErrorOr<RecordReleaseType>>
{
    public required Guid Id { get; init; }
    public required ReleaseType Type { get; init; }
    public string? Description { get; init; }
}

public class UpdateRecordReleaseTypeCommandHandler : IRequestHandler<UpdateRecordReleaseTypeCommand, ErrorOr<RecordReleaseType>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;

    public UpdateRecordReleaseTypeCommandHandler(IRecordReleaseTypeRepository recordReleaseTypeRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
    }

    public async Task<ErrorOr<RecordReleaseType>> Handle(UpdateRecordReleaseTypeCommand request, CancellationToken cancellationToken)
    {
        var recordReleaseTypeOption = await _recordReleaseTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (!recordReleaseTypeOption.HasValue)
        {
            return Errors.RecordReleaseType.NotFound(request.Id);
        }

        var recordReleaseType = recordReleaseTypeOption.ValueOr(() => throw new InvalidOperationException());
        recordReleaseType.UpdateType(request.Type, request.Description);
        await _recordReleaseTypeRepository.UpdateAsync(recordReleaseType, cancellationToken);

        return recordReleaseType;
    }
}
