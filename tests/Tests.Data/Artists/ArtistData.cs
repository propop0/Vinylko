using Domain.Artists;

namespace Tests.Data.Artists;

public static class ArtistData
{
    public static Artist FirstArtist()
    {
        return Artist.New(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            name: "The Beatles",
            bio: "Legendary English rock band formed in Liverpool in 1960.",
            country: "United Kingdom",
            birthDate: null,
            website: "https://thebeatles.com"
        );
    }

    public static Artist SecondArtist()
    {
        return Artist.New(
            id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            name: "Pink Floyd",
            bio: "English rock band known for progressive and psychedelic music.",
            country: "United Kingdom",
            birthDate: null,
            website: "https://pinkfloyd.com"
        );
    }

    public static Artist ThirdArtist()
    {
        return Artist.New(
            id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            name: "Led Zeppelin",
            bio: "English rock band formed in London in 1968.",
            country: "United Kingdom",
            birthDate: null,
            website: null
        );
    }

    public static Artist CreateArtistWithCustomName(string name)
    {
        return Artist.New(
            id: Guid.NewGuid(),
            name: name,
            bio: "Test artist biography",
            country: "Test Country",
            birthDate: null,
            website: null
        );
    }
}

