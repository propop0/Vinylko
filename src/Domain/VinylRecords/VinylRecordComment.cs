namespace Domain.VinylRecords;

public class VinylRecordComment
{
    public Guid Id { get; }
    public Guid VinylRecordId { get; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private VinylRecordComment(Guid id, Guid vinylRecordId, string content, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        VinylRecordId = vinylRecordId;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static VinylRecordComment New(Guid id, Guid vinylRecordId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Comment content cannot be empty.");
        }

        return new VinylRecordComment(id, vinylRecordId, content.Trim(), DateTime.UtcNow, null);
    }

    public void UpdateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Comment content cannot be empty.");
        }

        Content = content.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}


