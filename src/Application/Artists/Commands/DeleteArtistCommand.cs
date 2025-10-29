using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Artists.Commands
{
    public record DeleteArtistCommand : IRequest
    {
        public required Guid Id { get; init; }
    }

    public class DeleteArtistCommandHandler : IRequestHandler<DeleteArtistCommand>
    {
        private readonly IArtistRepository _artistRepository;

        public DeleteArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            // Check if artist has vinyl records before attempting deletion
            var hasVinylRecords = await _artistRepository.HasVinylRecordsAsync(request.Id, cancellationToken);
            if (hasVinylRecords)
            {
                throw new InvalidOperationException("Cannot delete artist. There are vinyl records associated with this artist. Please delete or reassign the vinyl records first.");
            }

            await _artistRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


