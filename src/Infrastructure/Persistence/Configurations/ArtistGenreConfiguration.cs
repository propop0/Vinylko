using Domain.ArtistGenres;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ArtistGenreConfiguration : IEntityTypeConfiguration<ArtistGenre>
{
    public void Configure(EntityTypeBuilder<ArtistGenre> builder)
    {
        builder.ToTable("artist_genres");

        builder.HasKey(x => new { x.ArtistId, x.GenreId });

        builder.Property(x => x.ArtistId)
            .IsRequired();

        builder.Property(x => x.GenreId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.HasOne<Domain.Artists.Artist>()
            .WithMany()
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Domain.Genres.Genre>()
            .WithMany()
            .HasForeignKey(x => x.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ArtistId);
        builder.HasIndex(x => x.GenreId);
    }
}

