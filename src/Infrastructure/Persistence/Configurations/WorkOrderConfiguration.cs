using Domain.WorkOrders;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WorkOrdersConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("work_order");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkOrderNumber)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(x => x.WorkOrderNumber).IsUnique();

        builder.Property(x => x.EquipmentId)
            .HasColumnName("equipment_id")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(1000)")
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.ScheduledDate)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.CompletedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        builder.Property(x => x.CompletionNotes)
            .HasColumnType("varchar(2000)")
            .IsRequired(false);

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
