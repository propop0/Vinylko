using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.VinylRecords.Commands
{
    public record DeleteVinylRecordCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
    }

    public class DeleteVinylRecordCommandHandler : IRequestHandler<DeleteVinylRecordCommand, ErrorOr<Success>>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public DeleteVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteVinylRecordCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!existingOption.HasValue)
            {
                return Errors.VinylRecord.NotFound(request.Id);
            }

            // Видалення дозволено навіть якщо є продажі - RecordId в продажах стане null через SetNull
            await _vinylRecordRepository.DeleteAsync(request.Id, cancellationToken);
            return Result.Success;
        }
    }
}
