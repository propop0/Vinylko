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
            await _artistRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


