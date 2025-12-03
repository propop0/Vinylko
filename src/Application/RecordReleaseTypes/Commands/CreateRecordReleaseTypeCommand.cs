using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.RecordReleaseTypes.Commands;

public record CreateRecordReleaseTypeCommand : IRequest<ErrorOr<RecordReleaseType>>
{
    public required Guid VinylRecordId { get; init; }
    public required ReleaseType Type { get; init; }
    public string? Description { get; init; }
}

public class CreateRecordReleaseTypeCommandHandler : IRequestHandler<CreateRecordReleaseTypeCommand, ErrorOr<RecordReleaseType>>
{
    private readonly IRecordReleaseTypeRepository _recordReleaseTypeRepository;
    private readonly IVinylRecordRepository _vinylRecordRepository;

    public CreateRecordReleaseTypeCommandHandler(
        IRecordReleaseTypeRepository recordReleaseTypeRepository,
        IVinylRecordRepository vinylRecordRepository)
    {
        _recordReleaseTypeRepository = recordReleaseTypeRepository;
        _vinylRecordRepository = vinylRecordRepository;
    }

    public async Task<ErrorOr<RecordReleaseType>> Handle(CreateRecordReleaseTypeCommand request, CancellationToken cancellationToken)
    {
        // Перевірка чи існує платівка
        var vinylRecordOption = await _vinylRecordRepository.GetByIdAsync(request.VinylRecordId, cancellationToken);
        if (!vinylRecordOption.HasValue)
        {
            return Errors.VinylRecord.NotFound(request.VinylRecordId);
        }

        // Перевірка чи вже є тип випуску для цієї платівки
        var existingOption = await _recordReleaseTypeRepository.GetByVinylRecordIdAsync(request.VinylRecordId, cancellationToken);
        if (existingOption.HasValue)
        {
            return Errors.RecordReleaseType.AlreadyExists(request.VinylRecordId);
        }

        var recordReleaseType = RecordReleaseType.New(
            Guid.NewGuid(),
            request.VinylRecordId,
            request.Type,
            request.Description);

        return await _recordReleaseTypeRepository.AddAsync(recordReleaseType, cancellationToken);
    }
}
