using Domain.VinylRecords;

namespace Tests.Data.VinylRecordComments;

public static class VinylRecordCommentData
{
    public static VinylRecordComment FirstComment(Guid vinylRecordId)
    {
        return VinylRecordComment.New(
            id: Guid.Parse("1111dddd-dddd-dddd-dddd-dddddddddddd"),
            vinylRecordId: vinylRecordId,
            content: "Amazing album! One of my favorites."
        );
    }

    public static VinylRecordComment SecondComment(Guid vinylRecordId)
    {
        return VinylRecordComment.New(
            id: Guid.Parse("2222eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            vinylRecordId: vinylRecordId,
            content: "Great sound quality on this pressing."
        );
    }

    public static VinylRecordComment ThirdComment(Guid vinylRecordId)
    {
        return VinylRecordComment.New(
            id: Guid.Parse("3333ffff-ffff-ffff-ffff-ffffffffffff"),
            vinylRecordId: vinylRecordId,
            content: "Classic record, highly recommended!"
        );
    }

    public static VinylRecordComment CreateCommentWithCustomContent(string content, Guid vinylRecordId)
    {
        return VinylRecordComment.New(
            id: Guid.NewGuid(),
            vinylRecordId: vinylRecordId,
            content: content
        );
    }
}

