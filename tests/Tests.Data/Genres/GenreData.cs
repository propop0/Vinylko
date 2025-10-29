using Domain.Genres;

namespace Tests.Data.Genres;

public static class GenreData
{
    public static Genre FirstGenre()
    {
        return Genre.New(
            id: Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
            name: "Rock",
            description: "A popular music genre characterized by a strong beat and amplified instruments."
        );
    }

    public static Genre SecondGenre()
    {
        return Genre.New(
            id: Guid.Parse("bbbb2222-2222-2222-2222-222222222222"),
            name: "Jazz",
            description: "A music genre that originated in African American communities in the late 19th and early 20th centuries."
        );
    }

    public static Genre ThirdGenre()
    {
        return Genre.New(
            id: Guid.Parse("cccc3333-3333-3333-3333-333333333333"),
            name: "Blues",
            description: "A music genre and musical form that originated in the Deep South of the United States around the 1860s."
        );
    }

    public static Genre CreateGenreWithCustomName(string name)
    {
        return Genre.New(
            id: Guid.NewGuid(),
            name: name,
            description: "Test genre description"
        );
    }
}

