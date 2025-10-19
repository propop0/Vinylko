using Domain.VinylRecords;

namespace Api.Dtos;

public record VinylRecordDto(
    Guid Id,
    string Title,
    string Genre,
    int ReleaseYear,
    string Status,
    Guid ArtistId,
    Guid LabelId,
    decimal Price,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static VinylRecordDto FromDomainModel(VinylRecord vinylRecord)
        => new(
            vinylRecord.Id,
            vinylRecord.Title,
            vinylRecord.Genre,
            vinylRecord.ReleaseYear,
            vinylRecord.Status.ToString(),
            vinylRecord.ArtistId,
            vinylRecord.LabelId,
            vinylRecord.Price,
            vinylRecord.Description,
            vinylRecord.CreatedAt,
            vinylRecord.UpdatedAt);
}

public record CreateVinylRecordDto(
    string Title,
    string Genre,
    int ReleaseYear,
    Guid ArtistId,
    Guid LabelId,
    decimal Price,
    string? Description = null);

public record UpdateVinylRecordDto(
    string Title,
    string Genre,
    int ReleaseYear,
    decimal Price,
    string? Description = null);

public record ChangeVinylRecordStatusDto(string Status);
