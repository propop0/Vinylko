using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Equipments.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/equipment")]
[ApiController]
public class EquipmentController(IEquipmentQueries equipmentQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EquipmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await equipmentQueries.GetAllAsync(cancellationToken);
        return items.Select(EquipmentDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EquipmentDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await equipmentQueries.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return EquipmentDto.FromDomainModel(item);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentDto>> Create([FromBody] CreateEquipmentDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CreateEquipmentCommand
        {
            Name = dto.Name,
            Model = dto.Model,
            SerialNumber = dto.SerialNumber,
            Location = dto.Location,
            InstallationDate = dto.InstallationDate
        };

        var created = await sender.Send(cmd, cancellationToken);
        return EquipmentDto.FromDomainModel(created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EquipmentDto>> Update(Guid id, [FromBody] UpdateEquipmentDto dto, CancellationToken cancellationToken)
    {
        var cmd = new UpdateEquipmentCommand
        {
            Id = id,
            Name = dto.Name,
            Model = dto.Model,
            Location = dto.Location
        };

        var updated = await sender.Send(cmd, cancellationToken);
        return EquipmentDto.FromDomainModel(updated);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeEquipmentStatusDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.Equipments.EquipmentStatus>(dto.Status, true, out var newStatus))
            return BadRequest("Invalid status value. Allowed: Operational, UnderMaintenance, OutOfService");

        var cmd = new ChangeEquipmentStatusCommand
        {
            Id = id,
            NewStatus = newStatus
        };

        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteEquipmentCommand(id);
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }
}
