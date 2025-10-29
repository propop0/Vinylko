using Domain.VinylRecords;

namespace Tests.Data.VinylRecords;

public static class VinylRecordData
{
    public static VinylRecord FirstVinylRecord(Guid artistId, Guid labelId)
    {
        return VinylRecord.New(
            id: Guid.Parse("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            title: "Abbey Road",
            genre: "Rock",
            releaseYear: 1969,
            artistId: artistId,
            labelId: labelId,
            price: 29.99m,
            description: "The Beatles' eleventh studio album."
        );
    }

    public static VinylRecord SecondVinylRecord(Guid artistId, Guid labelId)
    {
        return VinylRecord.New(
            id: Guid.Parse("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            title: "The Dark Side of the Moon",
            genre: "Progressive Rock",
            releaseYear: 1973,
            artistId: artistId,
            labelId: labelId,
            price: 34.99m,
            description: "Pink Floyd's iconic concept album."
        );
    }

    public static VinylRecord ThirdVinylRecord(Guid artistId, Guid labelId)
    {
        return VinylRecord.New(
            id: Guid.Parse("3333cccc-cccc-cccc-cccc-cccccccccccc"),
            title: "Led Zeppelin IV",
            genre: "Hard Rock",
            releaseYear: 1971,
            artistId: artistId,
            labelId: labelId,
            price: 32.50m,
            description: "One of the best-selling albums of all time."
        );
    }

    public static VinylRecord CreateVinylRecordWithCustomTitle(string title, Guid artistId, Guid labelId)
    {
        return VinylRecord.New(
            id: Guid.NewGuid(),
            title: title,
            genre: "Test Genre",
            releaseYear: 2000,
            artistId: artistId,
            labelId: labelId,
            price: 25.00m,
            description: "Test description"
        );
    }
}

