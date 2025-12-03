using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Artists;
using ErrorOr;
using MediatR;

namespace Application.Artists.Commands
{
    public record CreateArtistCommand : IRequest<ErrorOr<Artist>>
    {
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public required string Country { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Website { get; init; }
    }

    public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, ErrorOr<Artist>>
    {
        private readonly IArtistRepository _artistRepository;

        public CreateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ErrorOr<Artist>> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            var exists = await _artistRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (exists)
            {
                return Errors.Artist.AlreadyExists(request.Name);
            }

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


