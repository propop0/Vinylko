using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.Artists;
using MediatR;

namespace Application.Artists.Commands
{
    public record CreateArtistCommand : IRequest<Artist>
    {
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public required string Country { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Website { get; init; }
    }

    public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, Artist>
    {
        private readonly IArtistRepository _artistRepository;

        public CreateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<Artist> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            var artist = Artist.New(
                Guid.NewGuid(),
                request.Name,
                request.Bio,
                request.Country,
                request.BirthDate,
                request.Website);

            return await _artistRepository.AddAsync(artist, cancellationToken);
        }
    }
}


