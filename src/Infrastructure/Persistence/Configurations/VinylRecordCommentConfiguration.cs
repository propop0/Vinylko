using Domain.VinylRecords;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VinylRecordCommentConfiguration : IEntityTypeConfiguration<VinylRecordComment>
{
    public void Configure(EntityTypeBuilder<VinylRecordComment> builder)
    {
        builder.ToTable("vinyl_record_comments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VinylRecordId)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        builder.HasOne<VinylRecord>()
            .WithMany()
            .HasForeignKey(x => x.VinylRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.VinylRecordId);
        builder.HasIndex(x => x.CreatedAt);
    }
}


