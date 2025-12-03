using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.RecordReleaseTypes.Commands;

public record DeleteRecordReleaseTypeCommand : IRequest<ErrorOr<Success>>
{
    public required Guid Id { get; init; }
}

public class DeleteRecordReleaseTypeCommandHandler : IRequestHandler<DeleteRecordReleaseTypeCommand, ErrorOr<Success>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;

    public DeleteRecordReleaseTypeCommandHandler(IRecordReleaseTypeRepository recordReleaseTypeRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteRecordReleaseTypeCommand request, CancellationToken cancellationToken)
    {
        var recordReleaseTypeOption = await _recordReleaseTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (!recordReleaseTypeOption.HasValue)
        {
            return Errors.RecordReleaseType.NotFound(request.Id);
        }

        await _recordReleaseTypeRepository.DeleteAsync(request.Id, cancellationToken);
        return Result.Success;
    }
}
