namespace Domain.VinylRecords;

public class RecordReleaseType
{
    public Guid Id { get; }
    public Guid VinylRecordId { get; }
    public ReleaseType Type { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    // приватний конструктор
    private RecordReleaseType(
        Guid id,
        Guid vinylRecordId,
        ReleaseType type,
        string? description,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        VinylRecordId = vinylRecordId;
        Type = type;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static RecordReleaseType New(
        Guid id,
        Guid vinylRecordId,
        ReleaseType type,
        string? description = null)
    {
        return new RecordReleaseType(
            id,
            vinylRecordId,
            type,
            description,
            DateTime.UtcNow,
            null);
    }

    public void UpdateType(ReleaseType newType, string? description = null)
    {
        Type = newType;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum ReleaseType
{
    Official,              // Офіційний випуск
    Licensed,              // Ліцензована версія
    UnofficialOfficial,    // Сторонній виробник офіційного альбому
    Bootleg,               // Піратська версія
    UnreleasedSongs        // Версія з unreleased songs (неофіційні пісні)
}

