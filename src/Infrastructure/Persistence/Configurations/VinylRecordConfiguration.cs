using Domain.VinylRecords;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VinylRecordConfiguration : IEntityTypeConfiguration<VinylRecord>
{
    public void Configure(EntityTypeBuilder<VinylRecord> builder)
    {
        builder.ToTable("vinyl_records");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("varchar(300)")
            .IsRequired();

        builder.Property(x => x.Genre)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.ReleaseYear)
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.ArtistId)
            .IsRequired();

        builder.Property(x => x.LabelId)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        builder.HasOne<Domain.Artists.Artist>()
            .WithMany()
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Genre);
        builder.HasIndex(x => x.ReleaseYear);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ArtistId);
        builder.HasIndex(x => x.LabelId);
        builder.HasIndex(x => new { x.Title, x.ArtistId }).IsUnique();
    }
}
