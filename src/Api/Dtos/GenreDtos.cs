using Domain.Genres;

namespace Api.Dtos;

public record GenreDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static GenreDto FromDomainModel(Genre genre)
        => new(
            genre.Id,
            genre.Name,
            genre.Description,
            genre.CreatedAt,
            genre.UpdatedAt);
}

public record CreateGenreDto(
    string Name,
    string Description);

public record UpdateGenreDto(
    string Name,
    string Description);
