using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Artists.Commands
{
    public record UpdateArtistCommand : IRequest
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public required string Country { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Website { get; init; }
    }

    public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand>
    {
        private readonly IArtistRepository _artistRepository;

        public UpdateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
        {
            var existing = await _artistRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return;

            existing.UpdateDetails(request.Name, request.Bio, request.Country, request.BirthDate, request.Website);
            await _artistRepository.UpdateAsync(existing, cancellationToken);
        }
    }
}


