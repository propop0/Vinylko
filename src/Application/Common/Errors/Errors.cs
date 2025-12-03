using ErrorOr;

namespace Application.Common.Errors;

public static class Errors
{
    public static class Artist
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "Artist.NotFound",
            $"Artist with id {id} was not found.");

        public static Error AlreadyExists(string name) => Error.Conflict(
            "Artist.AlreadyExists",
            $"Artist with name {name} already exists.");

        public static Error HasVinylRecords(Guid id) => Error.Conflict(
            "Artist.HasVinylRecords",
            $"Cannot delete artist {id} because it has associated vinyl records.");
    }

    public static class Genre
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "Genre.NotFound",
            $"Genre with id {id} was not found.");

        public static Error AlreadyExists(string name) => Error.Conflict(
            "Genre.AlreadyExists",
            $"Genre with name {name} already exists.");

        public static Error HasVinylRecords(Guid id) => Error.Conflict(
            "Genre.HasVinylRecords",
            $"Cannot delete genre {id} because it has associated vinyl records.");
    }

    public static class VinylRecord
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "VinylRecord.NotFound",
            $"VinylRecord with id {id} was not found.");

        public static Error AlreadyExists(string title, Guid artistId) => Error.Conflict(
            "VinylRecord.AlreadyExists",
            $"VinylRecord with title {title} and artist {artistId} already exists.");
    }

    public static class Sale
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "Sale.NotFound",
            $"Sale with id {id} was not found.");

        public static Error InvalidStatus(string currentStatus, string targetStatus) => Error.Validation(
            "Sale.InvalidStatus",
            $"Cannot change sale status from {currentStatus} to {targetStatus}.");

        public static Error SaleNumberAlreadyExists(string saleNumber) => Error.Conflict(
            "Sale.SaleNumberAlreadyExists",
            $"Sale with number {saleNumber} already exists.");
    }

    public static class ArtistGenre
    {
        public static Error NotFound(Guid artistId, Guid genreId) => Error.NotFound(
            "ArtistGenre.NotFound",
            $"Artist {artistId} does not have genre {genreId}.");

        public static Error AlreadyExists(Guid artistId, Guid genreId) => Error.Conflict(
            "ArtistGenre.AlreadyExists",
            $"Artist {artistId} already has genre {genreId}.");
    }

    public static class RecordReleaseType
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "RecordReleaseType.NotFound",
            $"RecordReleaseType with id {id} was not found.");

        public static Error AlreadyExists(Guid vinylRecordId) => Error.Conflict(
            "RecordReleaseType.AlreadyExists",
            $"RecordReleaseType already exists for VinylRecord {vinylRecordId}.");
    }

    public static class VinylRecordComment
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "VinylRecordComment.NotFound",
            $"VinylRecordComment with id {id} was not found.");

        public static Error VinylRecordNotFound(Guid vinylRecordId) => Error.NotFound(
            "VinylRecordComment.VinylRecordNotFound",
            $"VinylRecord with id {vinylRecordId} was not found.");
    }
}

