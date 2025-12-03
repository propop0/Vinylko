using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Artists;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Artists.Commands
{
    public record UpdateArtistCommand : IRequest<ErrorOr<Artist>>
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Bio { get; init; }
        public required string Country { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Website { get; init; }
    }

    public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand, ErrorOr<Artist>>
    {
        private readonly IArtistRepository _artistRepository;

        public UpdateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ErrorOr<Artist>> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _artistRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (!existingOption.HasValue)
            {
                return Errors.Artist.NotFound(request.Id);
            }

            var artist = existingOption.ValueOr(() => throw new InvalidOperationException());
            artist.UpdateDetails(request.Name, request.Bio, request.Country, request.BirthDate, request.Website);
            await _artistRepository.UpdateAsync(artist, cancellationToken);
            return artist;
        }
    }
}
