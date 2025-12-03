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
    public record UpdateVinylRecordCommand : IRequest<ErrorOr<VinylRecord>>
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Genre { get; init; }
        public required int ReleaseYear { get; init; }
        public required decimal Price { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateVinylRecordCommandHandler : IRequestHandler<UpdateVinylRecordCommand, ErrorOr<VinylRecord>>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public UpdateVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<ErrorOr<VinylRecord>> Handle(UpdateVinylRecordCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (!existingOption.HasValue)
            {
                return Errors.VinylRecord.NotFound(request.Id);
            }

            var vinylRecord = existingOption.ValueOr(() => throw new InvalidOperationException());
            vinylRecord.UpdateDetails(request.Title, request.Genre, request.ReleaseYear, request.Price, request.Description);
            await _vinylRecordRepository.UpdateAsync(vinylRecord, cancellationToken);
            return vinylRecord;
        }
    }
}
