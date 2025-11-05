using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.VinylRecordComments.Commands;
using Application.VinylRecordComments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/vinyl-record-comments")] 
public class VinylRecordCommentsController(IVinylRecordCommentQueries queries, ISender sender) : ControllerBase
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
        try
        {
            var created = await sender.Send(new CreateVinylRecordCommentCommand
            {
                VinylRecordId = id,
                Content = dto.Content
            }, cancellationToken);

            return CreatedAtAction(nameof(GetForVinyl), new { id = created.VinylRecordId }, VinylRecordCommentDto.FromDomainModel(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VinylRecordCommentDto>> Update(Guid id, [FromBody] UpdateVinylRecordCommentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await sender.Send(new UpdateVinylRecordCommentCommand
            {
                Id = id,
                Content = dto.Content
            }, cancellationToken);
            return VinylRecordCommentDto.FromDomainModel(updated);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteVinylRecordCommentCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}