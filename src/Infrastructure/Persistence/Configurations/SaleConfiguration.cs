using Domain.Sales;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sales");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SaleNumber)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.RecordId)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.SaleDate)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(x => x.CustomerName)
            .HasColumnType("varchar(200)")
            .IsRequired(false);

        builder.Property(x => x.CustomerEmail)
            .HasColumnType("varchar(300)")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        // Foreign key relationship with proper delete behavior
        builder.HasOne<Domain.VinylRecords.VinylRecord>()
            .WithMany()
            .HasForeignKey(x => x.RecordId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of vinyl record if sales exist

        builder.HasIndex(x => x.SaleNumber).IsUnique();
        builder.HasIndex(x => x.RecordId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.SaleDate);
        builder.HasIndex(x => x.CustomerEmail);
    }
}
