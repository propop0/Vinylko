using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.VinylRecords.Commands
{
    public record ChangeVinylRecordStatusCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
        public required VinylRecordStatus Status { get; init; }
    }

    public class ChangeVinylRecordStatusCommandHandler : IRequestHandler<ChangeVinylRecordStatusCommand, ErrorOr<Success>>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public ChangeVinylRecordStatusCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<ErrorOr<Success>> Handle(ChangeVinylRecordStatusCommand request, CancellationToken cancellationToken)
        {
            var entityOption = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (!entityOption.HasValue)
            {
                return Errors.VinylRecord.NotFound(request.Id);
            }

            var entity = entityOption.ValueOr(() => throw new InvalidOperationException());
            entity.ChangeStatus(request.Status);
            await _vinylRecordRepository.UpdateAsync(entity, cancellationToken);
            return Result.Success;
        }
    }
}
