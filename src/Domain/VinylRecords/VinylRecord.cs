namespace Domain.VinylRecords;

public class VinylRecord
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Genre { get; private set; }
    public int ReleaseYear { get; private set; }
    public VinylRecordStatus Status { get; private set; }
    public Guid ArtistId { get; private set; }
    public Guid LabelId { get; private set; }
    public decimal Price { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    // приватний конструктор
    private VinylRecord(Guid id,
        string title,
        string genre,
        int releaseYear,
        VinylRecordStatus status,
        Guid artistId,
        Guid labelId,
        decimal price,
        string? description,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Title = title;
        Genre = genre;
        ReleaseYear = releaseYear;
        Status = status;
        ArtistId = artistId;
        LabelId = labelId;
        Price = price;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static VinylRecord New(
        Guid id,
        string title,
        string genre,
        int releaseYear,
        Guid artistId,
        Guid labelId,
        decimal price,
        string? description = null)
    {
        return new VinylRecord(
            id,
            title,
            genre,
            releaseYear,
            VinylRecordStatus.InStock,
            artistId,
            labelId,
            price,
            description,
            DateTime.UtcNow,
            null);
    }

    public void UpdateDetails(string title, string genre, int releaseYear, decimal price, string? description = null)
    {
        Title = title;
        Genre = genre;
        ReleaseYear = releaseYear;
        Price = price;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(VinylRecordStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reserve()
    {
        if (Status == VinylRecordStatus.InStock)
        {
            Status = VinylRecordStatus.Reserved;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Sell()
    {
        if (Status == VinylRecordStatus.InStock || Status == VinylRecordStatus.Reserved)
        {
            Status = VinylRecordStatus.Sold;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

public enum VinylRecordStatus
{
    InStock,
    Reserved,
    Sold
}
