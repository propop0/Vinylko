namespace Domain.ArtistGenres;

public class ArtistGenre
{
    public Guid ArtistId { get; }
    public Guid GenreId { get; }
    public DateTime CreatedAt { get; }

    // приватний конструктор
    private ArtistGenre(Guid artistId, Guid genreId, DateTime createdAt)
    {
        ArtistId = artistId;
        GenreId = genreId;
        CreatedAt = createdAt;
    }

    public static ArtistGenre New(Guid artistId, Guid genreId)
    {
        return new ArtistGenre(artistId, genreId, DateTime.UtcNow);
    }
}

