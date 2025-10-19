using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.WorkOrders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/work-orders")]
[ApiController]
public class WorkOrdersController(IWorkOrderQueries queries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WorkOrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await queries.GetAllAsync(cancellationToken);
        return items.Select(WorkOrderDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkOrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await queries.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return WorkOrderDto.FromDomainModel(item);
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderDto>> Create([FromBody] CreateWorkOrderDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.WorkOrders.WorkOrderPriority>(dto.Priority, true, out var priority))
            return BadRequest("Invalid priority");

        var cmd = new CreateWorkOrderCommand
        {
            EquipmentId = dto.EquipmentId,
            Title = dto.Title,
            Description = dto.Description,
            Priority = priority,
            ScheduledDate = dto.ScheduledDate
        };

        var created = await sender.Send(cmd, cancellationToken);
        return WorkOrderDto.FromDomainModel(created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WorkOrderDto>> Update(Guid id, [FromBody] UpdateWorkOrderDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.WorkOrders.WorkOrderPriority>(dto.Priority, true, out var priority))
            return BadRequest("Invalid priority");

        var cmd = new UpdateWorkOrderCommand
        {
            Id = id,
            Title = dto.Title,
            Description = dto.Description,
            Priority = priority,
            ScheduledDate = dto.ScheduledDate
        };

        var updated = await sender.Send(cmd, cancellationToken);
        return WorkOrderDto.FromDomainModel(updated);
    }

    [HttpPatch("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new StartWorkOrderCommand(id);
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteWorkOrderDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CompleteWorkOrderCommand(id, dto.CompletionNotes);
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new CancelWorkOrderCommand(id);
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }
}
