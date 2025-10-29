using Domain.Genres;

namespace Tests.Data.Genres;

public static class GenreData
{
    public static Genre GetRandomGenre()
    {
        var uniqueId = Guid.NewGuid(); 
        var uniqueName = $"Test Genre {uniqueId.ToString().Substring(0, 8)}"; 

        return Genre.New(
            id: uniqueId,
            name: uniqueName,
            description: $"Description for {uniqueName}"
        );
    }
    
    public static Genre FirstGenre() => GetRandomGenre();
    public static Genre SecondGenre() => GetRandomGenre();
    public static Genre ThirdGenre() => GetRandomGenre();

    public static Genre CreateGenreWithCustomName(string name)
    {
        return Genre.New(
            id: Guid.NewGuid(),
            name: name,
            description: "Test genre description"
        );
    }
}

