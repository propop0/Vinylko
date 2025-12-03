using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace Application.ArtistGenres.Commands;

public record RemoveGenreFromArtistCommand : IRequest<ErrorOr<Success>>
{
    public required Guid ArtistId { get; init; }
    public required Guid GenreId { get; init; }
}

public class RemoveGenreFromArtistCommandHandler : IRequestHandler<RemoveGenreFromArtistCommand, ErrorOr<Success>>
{
    private readonly IArtistGenreRepository _artistGenreRepository;

    public RemoveGenreFromArtistCommandHandler(IArtistGenreRepository artistGenreRepository)
    {
        _artistGenreRepository = artistGenreRepository;
    }

    public async Task<ErrorOr<Success>> Handle(RemoveGenreFromArtistCommand request, CancellationToken cancellationToken)
    {
        var exists = await _artistGenreRepository.ExistsAsync(request.ArtistId, request.GenreId, cancellationToken);
        if (!exists)
        {
            return Errors.ArtistGenre.NotFound(request.ArtistId, request.GenreId);
        }

        await _artistGenreRepository.DeleteAsync(request.ArtistId, request.GenreId, cancellationToken);
        return Result.Success;
    }
}
