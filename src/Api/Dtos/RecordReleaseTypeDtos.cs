using Domain.VinylRecords;

namespace Api.Dtos;

public record RecordReleaseTypeDto(
    Guid Id,
    Guid VinylRecordId,
    ReleaseType Type,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static RecordReleaseTypeDto FromDomainModel(RecordReleaseType recordReleaseType)
        => new(
            recordReleaseType.Id,
            recordReleaseType.VinylRecordId,
            recordReleaseType.Type,
            recordReleaseType.Description,
            recordReleaseType.CreatedAt,
            recordReleaseType.UpdatedAt);
}

public record CreateRecordReleaseTypeDto(
    Guid VinylRecordId,
    ReleaseType Type,
    string? Description = null);

public record UpdateRecordReleaseTypeDto(
    ReleaseType Type,
    string? Description = null);

