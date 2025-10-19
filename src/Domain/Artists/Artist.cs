namespace Domain.Artists;

public class Artist
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Bio { get; private set; }
    public string Country { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? Website { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    // приватний конструктор
    private Artist(Guid id,
        string name,
        string bio,
        string country,
        DateTime? birthDate,
        string? website,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        Bio = bio;
        Country = country;
        BirthDate = birthDate;
        Website = website;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Artist New(
        Guid id,
        string name,
        string bio,
        string country,
        DateTime? birthDate = null,
        string? website = null)
    {
        return new Artist(
            id,
            name,
            bio,
            country,
            birthDate,
            website,
            DateTime.UtcNow,
            null);
    }

    public void UpdateDetails(
        string name,
        string bio,
        string country,
        DateTime? birthDate = null,
        string? website = null)
    {
        Name = name;
        Bio = bio;
        Country = country;
        BirthDate = birthDate;
        Website = website;
        UpdatedAt = DateTime.UtcNow;
    }
}
