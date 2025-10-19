using Domain.WorkOrders;

namespace Api.Dtos;

public record WorkOrderDto(
    Guid Id,
    string WorkOrderNumber,
    Guid EquipmentId,
    string Title,
    string Description,
    string Priority,
    string Status,
    DateTime ScheduledDate,
    DateTime? CompletedAt,
    string? CompletionNotes,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static WorkOrderDto FromDomainModel(WorkOrder w)
        => new(
            w.Id,
            w.WorkOrderNumber,
            w.EquipmentId,
            w.Title,
            w.Description,
            w.Priority.ToString(),
            w.Status.ToString(),
            w.ScheduledDate,
            w.CompletedAt,
            w.CompletionNotes,
            w.CreatedAt,
            w.UpdatedAt);
}

public record CreateWorkOrderDto(Guid EquipmentId, string Title, string Description, string Priority, DateTime ScheduledDate);
public record UpdateWorkOrderDto(string Title, string Description, string Priority, DateTime ScheduledDate);
public record CompleteWorkOrderDto(string CompletionNotes);