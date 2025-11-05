using Domain.VinylRecords;

namespace Api.Dtos;

public record VinylRecordCommentDto
{
    public required Guid Id { get; init; }
    public required Guid VinylRecordId { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static VinylRecordCommentDto FromDomainModel(VinylRecordComment comment) => new()
    {
        Id = comment.Id,
        VinylRecordId = comment.VinylRecordId,
        Content = comment.Content,
        CreatedAt = comment.CreatedAt,
        UpdatedAt = comment.UpdatedAt
    };
}

public record CreateVinylRecordCommentDto
{
    public required string Content { get; init; }
}

public record UpdateVinylRecordCommentDto
{
    public required string Content { get; init; }
}


