using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Commands
{
    public record CreateVinylRecordCommand : IRequest<VinylRecord>
    {
        public required string Title { get; init; }
        public required string Genre { get; init; }
        public required int ReleaseYear { get; init; }
        public required Guid ArtistId { get; init; }
        public required Guid LabelId { get; init; }
        public required decimal Price { get; init; }
        public string? Description { get; init; }
    }

    public class CreateVinylRecordCommandHandler : IRequestHandler<CreateVinylRecordCommand, VinylRecord>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public CreateVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<VinylRecord> Handle(CreateVinylRecordCommand request, CancellationToken cancellationToken)
        {
            var vinyl = VinylRecord.New(
                Guid.NewGuid(),
                request.Title,
                request.Genre,
                request.ReleaseYear,
                request.ArtistId,
                request.LabelId,
                request.Price,
                request.Description);

            return await _vinylRecordRepository.AddAsync(vinyl, cancellationToken);
        }
    }
}


