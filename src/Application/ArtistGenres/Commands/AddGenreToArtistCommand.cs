using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.ArtistGenres;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.ArtistGenres.Commands;

public record AddGenreToArtistCommand : IRequest<ErrorOr<ArtistGenre>>
{
    public required Guid ArtistId { get; init; }
    public required Guid GenreId { get; init; }
}

public class AddGenreToArtistCommandHandler : IRequestHandler<AddGenreToArtistCommand, ErrorOr<ArtistGenre>>
{
    private readonly IArtistGenreRepository _artistGenreRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IGenreRepository _genreRepository;

    public AddGenreToArtistCommandHandler(
        IArtistGenreRepository artistGenreRepository,
        IArtistRepository artistRepository,
        IGenreRepository genreRepository)
    {
        _artistGenreRepository = artistGenreRepository;
        _artistRepository = artistRepository;
        _genreRepository = genreRepository;
    }

    public async Task<ErrorOr<ArtistGenre>> Handle(AddGenreToArtistCommand request, CancellationToken cancellationToken)
    {
        // Перевірка чи існує артист
        var artistOption = await _artistRepository.GetByIdAsync(request.ArtistId, cancellationToken);
        if (!artistOption.HasValue)
        {
            return Errors.Artist.NotFound(request.ArtistId);
        }

        // Перевірка чи існує жанр
        var genreOption = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken);
        if (!genreOption.HasValue)
        {
            return Errors.Genre.NotFound(request.GenreId);
        }

        // Перевірка чи зв'язок вже існує
        var exists = await _artistGenreRepository.ExistsAsync(request.ArtistId, request.GenreId, cancellationToken);
        if (exists)
        {
            return Errors.ArtistGenre.AlreadyExists(request.ArtistId, request.GenreId);
        }

        var artistGenre = ArtistGenre.New(request.ArtistId, request.GenreId);
        return await _artistGenreRepository.AddAsync(artistGenre, cancellationToken);
    }
}
