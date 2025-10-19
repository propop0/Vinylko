using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Commands
{
    public record UpdateVinylRecordCommand : IRequest
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Genre { get; init; }
        public required int ReleaseYear { get; init; }
        public required decimal Price { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateVinylRecordCommandHandler : IRequestHandler<UpdateVinylRecordCommand>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public UpdateVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task Handle(UpdateVinylRecordCommand request, CancellationToken cancellationToken)
        {
            var existing = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return;

            existing.UpdateDetails(request.Title, request.Genre, request.ReleaseYear, request.Price, request.Description);
            await _vinylRecordRepository.UpdateAsync(existing, cancellationToken);
        }
    }
}


