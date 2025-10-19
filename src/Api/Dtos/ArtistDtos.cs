using Domain.Artists;

namespace Api.Dtos;

public record ArtistDto(
    Guid Id,
    string Name,
    string Bio,
    string Country,
    DateTime? BirthDate,
    string? Website,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ArtistDto FromDomainModel(Artist artist)
        => new(
            artist.Id,
            artist.Name,
            artist.Bio,
            artist.Country,
            artist.BirthDate,
            artist.Website,
            artist.CreatedAt,
            artist.UpdatedAt);
}

public record CreateArtistDto(
    string Name,
    string Bio,
    string Country,
    DateTime? BirthDate = null,
    string? Website = null);

public record UpdateArtistDto(
    string Name,
    string Bio,
    string Country,
    DateTime? BirthDate = null,
    string? Website = null);
