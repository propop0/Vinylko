using Domain.MaintenanceSchedules;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MaintenanceScheduleConfiguration : IEntityTypeConfiguration<MaintenanceSchedule>
{
    public void Configure(EntityTypeBuilder<MaintenanceSchedule> builder)
    {
        builder.ToTable("maintenance_schedule");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EquipmentId)
            .HasColumnName("equipment_id")
            .IsRequired();

        builder.HasIndex(x => x.EquipmentId);

        builder.Property(x => x.TaskName)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(x => x.Frequency)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.NextDueDate)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        builder.HasOne<Domain.Equipments.Equipment>()
            .WithMany()
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}