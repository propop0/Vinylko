using Domain.Equipments;

namespace Api.Dtos;

public record EquipmentDto(
    Guid Id,
    string Name,
    string Model,
    string SerialNumber,
    string Location,
    string Status,
    DateTime InstallationDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static EquipmentDto FromDomainModel(Equipment e)
        => new(
            e.Id,
            e.Name,
            e.Model,
            e.SerialNumber,
            e.Location,
            e.Status.ToString(),
            e.InstallationDate,
            e.CreatedAt,
            e.UpdatedAt);
}

public record CreateEquipmentDto(string Name, string Model, string SerialNumber, string Location, DateTime InstallationDate);

public record UpdateEquipmentDto(string Name, string Model, string Location);

public record ChangeEquipmentStatusDto(string Status);