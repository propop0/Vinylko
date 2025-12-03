using Domain.ArtistGenres;

namespace Api.Dtos;

public record ArtistGenreDto(
    Guid ArtistId,
    Guid GenreId,
    DateTime CreatedAt)
{
    public static ArtistGenreDto FromDomainModel(ArtistGenre artistGenre)
        => new(
            artistGenre.ArtistId,
            artistGenre.GenreId,
            artistGenre.CreatedAt);
}

public record AddGenreToArtistDto(
    Guid ArtistId,
    Guid GenreId);

