using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.MaintenanceSchedules.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/maintenance-schedules")]
[ApiController]
public class MaintenanceSchedulesController(IMaintenanceScheduleQueries scheduleQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceScheduleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await scheduleQueries.GetAllAsync(cancellationToken);
        return items.Select(MaintenanceScheduleDto.FromDomainModel).ToList();
    }

    [HttpGet("equipment/{equipmentId:guid}")]
    public async Task<ActionResult<IReadOnlyList<MaintenanceScheduleDto>>> GetByEquipment(Guid equipmentId, CancellationToken cancellationToken)
    {
        var items = await scheduleQueries.GetByEquipmentIdAsync(equipmentId, cancellationToken);
        return items.Select(MaintenanceScheduleDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceScheduleDto>> Create([FromBody] CreateMaintenanceScheduleDto dto, CancellationToken cancellationToken)
    {
        // parse frequency enum
        if (!Enum.TryParse<Domain.MaintenanceSchedules.MaintenanceFrequency>(dto.Frequency, true, out var freq))
            return BadRequest("Invalid frequency");

        var cmd = new CreateMaintenanceScheduleCommand
        {
            EquipmentId = dto.EquipmentId,
            TaskName = dto.TaskName,
            Description = dto.Description,
            Frequency = freq,
            NextDueDate = dto.NextDueDate
        };

        var created = await sender.Send(cmd, cancellationToken);
        return MaintenanceScheduleDto.FromDomainModel(created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MaintenanceScheduleDto>> Update(Guid id, [FromBody] UpdateMaintenanceScheduleDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.MaintenanceSchedules.MaintenanceFrequency>(dto.Frequency, true, out var freq))
            return BadRequest("Invalid frequency");

        var cmd = new UpdateMaintenanceScheduleCommand
        {
            Id = id,
            TaskName = dto.TaskName,
            Description = dto.Description,
            Frequency = freq,
            NextDueDate = dto.NextDueDate
        };

        var updated = await sender.Send(cmd, cancellationToken);
        return MaintenanceScheduleDto.FromDomainModel(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeactivateMaintenanceScheduleCommand(id);
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }
}
