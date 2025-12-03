using System;
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
    public record CreateVinylRecordCommand : IRequest<ErrorOr<VinylRecord>>
    {
        public required string Title { get; init; }
        public required string Genre { get; init; }
        public required int ReleaseYear { get; init; }
        public required Guid ArtistId { get; init; }
        public required decimal Price { get; init; }
        public string? Description { get; init; }
    }

    public class CreateVinylRecordCommandHandler : IRequestHandler<CreateVinylRecordCommand, ErrorOr<VinylRecord>>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;
        private readonly IArtistRepository _artistRepository;

        public CreateVinylRecordCommandHandler(
            IVinylRecordRepository vinylRecordRepository,
            IArtistRepository artistRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
            _artistRepository = artistRepository;
        }

        public async Task<ErrorOr<VinylRecord>> Handle(CreateVinylRecordCommand request, CancellationToken cancellationToken)
        {
            // Перевірка чи існує артист
            var artistOption = await _artistRepository.GetByIdAsync(request.ArtistId, cancellationToken);
            if (!artistOption.HasValue)
            {
                return Errors.Artist.NotFound(request.ArtistId);
            }

            // Перевірка на унікальність (Title + ArtistId)
            var exists = await _vinylRecordRepository.ExistsByTitleAndArtistAsync(request.Title, request.ArtistId, cancellationToken);
            if (exists)
            {
                return Errors.VinylRecord.AlreadyExists(request.Title, request.ArtistId);
            }

            var vinyl = VinylRecord.New(
                Guid.NewGuid(),
                request.Title,
                request.Genre,
                request.ReleaseYear,
                request.ArtistId,
                request.Price,
                request.Description);

            return await _vinylRecordRepository.AddAsync(vinyl, cancellationToken);
        }
    }
}


