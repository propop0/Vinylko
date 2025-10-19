using Domain.MaintenanceSchedules;

namespace Api.Dtos;

public record MaintenanceScheduleDto(
    Guid Id,
    Guid EquipmentId,
    string TaskName,
    string Description,
    string Frequency,
    DateTime NextDueDate,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static MaintenanceScheduleDto FromDomainModel(MaintenanceSchedule s)
        => new(
            s.Id,
            s.EquipmentId,
            s.TaskName,
            s.Description,
            s.Frequency.ToString(),
            s.NextDueDate,
            s.IsActive,
            s.CreatedAt,
            s.UpdatedAt);
}

public record CreateMaintenanceScheduleDto(Guid EquipmentId, string TaskName, string Description, string Frequency, DateTime NextDueDate);

public record UpdateMaintenanceScheduleDto(string TaskName, string Description, string Frequency, DateTime NextDueDate);