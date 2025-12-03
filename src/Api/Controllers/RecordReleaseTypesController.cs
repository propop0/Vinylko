using Api.Dtos;
using Application.RecordReleaseTypes.Commands;
using Application.RecordReleaseTypes.Queries;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace Api.Controllers;

[Route("api/record-release-types")]
[ApiController]
public class RecordReleaseTypesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RecordReleaseTypeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetRecordReleaseTypesQuery();
        var items = await sender.Send(query, cancellationToken);
        return items.Select(RecordReleaseTypeDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecordReleaseTypeDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRecordReleaseTypeByIdQuery { Id = id };
        var itemOption = await sender.Send(query, cancellationToken);
        return itemOption.Match<ActionResult<RecordReleaseTypeDto>>(
            some: item => RecordReleaseTypeDto.FromDomainModel(item),
            none: () => NotFound());
    }

    [HttpGet("vinyl-record/{vinylRecordId:guid}")]
    public async Task<ActionResult<RecordReleaseTypeDto>> GetByVinylRecordId(
        Guid vinylRecordId,
        CancellationToken cancellationToken)
    {
        var query = new GetRecordReleaseTypeByVinylRecordIdQuery { VinylRecordId = vinylRecordId };
        var itemOption = await sender.Send(query, cancellationToken);
        return itemOption.Match<ActionResult<RecordReleaseTypeDto>>(
            some: item => RecordReleaseTypeDto.FromDomainModel(item),
            none: () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<RecordReleaseTypeDto>> Create(
        [FromBody] CreateRecordReleaseTypeDto dto,
        CancellationToken cancellationToken)
    {
        var cmd = new CreateRecordReleaseTypeCommand
        {
            VinylRecordId = dto.VinylRecordId,
            Type = dto.Type,
            Description = dto.Description
        };

        var result = await sender.Send(cmd, cancellationToken);
        
        return result.Match<ActionResult<RecordReleaseTypeDto>>(
            value => CreatedAtAction(
                nameof(GetById),
                new { id = value.Id },
                RecordReleaseTypeDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(errors),
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RecordReleaseTypeDto>> Update(
        Guid id,
        [FromBody] UpdateRecordReleaseTypeDto dto,
        CancellationToken cancellationToken)
    {
        var cmd = new UpdateRecordReleaseTypeCommand
        {
            Id = id,
            Type = dto.Type,
            Description = dto.Description
        };

        var result = await sender.Send(cmd, cancellationToken);
        
        return result.Match<ActionResult<RecordReleaseTypeDto>>(
            value => RecordReleaseTypeDto.FromDomainModel(value),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteRecordReleaseTypeCommand { Id = id };
        var result = await sender.Send(cmd, cancellationToken);
        
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(errors),
                _ => BadRequest(errors)
            });
    }
}

