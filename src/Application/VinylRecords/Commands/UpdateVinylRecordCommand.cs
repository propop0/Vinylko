using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Commands
{
    public record UpdateVinylRecordCommand : IRequest<VinylRecord>
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Genre { get; init; }
        public required int ReleaseYear { get; init; }
        public required decimal Price { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateVinylRecordCommandHandler : IRequestHandler<UpdateVinylRecordCommand, VinylRecord>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public UpdateVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<VinylRecord> Handle(UpdateVinylRecordCommand request, CancellationToken cancellationToken)
        {
            var existing = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new InvalidOperationException("Vinyl record not found");

            existing.UpdateDetails(request.Title, request.Genre, request.ReleaseYear, request.Price, request.Description);
            await _vinylRecordRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


