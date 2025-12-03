using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.VinylRecordComments.Commands;
using Application.VinylRecordComments.Queries;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/vinyl-record-comments")] 
public class VinylRecordCommentsController(ISender sender) : ControllerBase
{
    [HttpGet("/api/vinyl-records/{id:guid}/comments")] 
    public async Task<ActionResult<IReadOnlyList<VinylRecordCommentDto>>> GetForVinyl(Guid id, CancellationToken cancellationToken)
    {
        var items = await sender.Send(new GetCommentsForVinylRecordQuery { VinylRecordId = id }, cancellationToken);
        return items.Select(VinylRecordCommentDto.FromDomainModel).ToList();
    }

    [HttpPost("/api/vinyl-records/{id:guid}/comments")]
    public async Task<ActionResult<VinylRecordCommentDto>> Create(Guid id, [FromBody] CreateVinylRecordCommentDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateVinylRecordCommentCommand
        {
            VinylRecordId = id,
            Content = dto.Content
        }, cancellationToken);

        return result.Match<ActionResult<VinylRecordCommentDto>>(
            value => CreatedAtAction(nameof(GetForVinyl), new { id = value.VinylRecordId }, VinylRecordCommentDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VinylRecordCommentDto>> Update(Guid id, [FromBody] UpdateVinylRecordCommentDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateVinylRecordCommentCommand
        {
            Id = id,
            Content = dto.Content
        }, cancellationToken);

        return result.Match<ActionResult<VinylRecordCommentDto>>(
            value => VinylRecordCommentDto.FromDomainModel(value),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteVinylRecordCommentCommand { Id = id }, cancellationToken);
        
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }
}