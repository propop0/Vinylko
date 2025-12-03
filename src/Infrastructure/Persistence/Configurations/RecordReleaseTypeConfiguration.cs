using Domain.VinylRecords;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RecordReleaseTypeConfiguration : IEntityTypeConfiguration<RecordReleaseType>
{
    public void Configure(EntityTypeBuilder<RecordReleaseType> builder)
    {
        builder.ToTable("record_release_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VinylRecordId)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
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

        // One-to-One зв'язок з VinylRecord
        builder.HasOne<VinylRecord>()
            .WithOne()
            .HasForeignKey<RecordReleaseType>(x => x.VinylRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.VinylRecordId).IsUnique();
        builder.HasIndex(x => x.Type);
    }
}

