using Domain.Equipments;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("equipment");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.Model)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.HasIndex(x => x.SerialNumber).IsUnique();

        builder.Property(x => x.Location)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.InstallationDate)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);
    }
}