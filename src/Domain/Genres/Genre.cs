namespace Domain.Genres;

public class Genre
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    // приватний конструктор
    private Genre(Guid id,
        string name,
        string description,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Genre New(
        Guid id,
        string name,
        string description)
    {
        return new Genre(
            id,
            name,
            description,
            DateTime.UtcNow,
            null);
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
