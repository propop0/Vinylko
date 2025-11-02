using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.VinylRecords.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Controllers;

[Route("api/vinyl-records")]
[ApiController]
public class VinylRecordsController(IVinylRecordQueries vinylRecordQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VinylRecordDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await vinylRecordQueries.GetAllAsync(cancellationToken);
        return items.Select(VinylRecordDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VinylRecordDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await vinylRecordQueries.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return VinylRecordDto.FromDomainModel(item);
    }

    [HttpGet("artist/{artistId:guid}")]
    public async Task<ActionResult<IReadOnlyList<VinylRecordDto>>> GetByArtist(Guid artistId, CancellationToken cancellationToken)
    {
        var items = await vinylRecordQueries.GetByArtistIdAsync(artistId, cancellationToken);
        return items.Select(VinylRecordDto.FromDomainModel).ToList();
    }

    [HttpGet("genre/{genreId:guid}")]
    public async Task<ActionResult<IReadOnlyList<VinylRecordDto>>> GetByGenre(Guid genreId, CancellationToken cancellationToken)
    {
        var items = await vinylRecordQueries.GetByGenreIdAsync(genreId, cancellationToken);
        return items.Select(VinylRecordDto.FromDomainModel).ToList();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<VinylRecordDto>>> GetByStatus(string status, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.VinylRecords.VinylRecordStatus>(status, true, out var vinylStatus))
            return BadRequest("Invalid status value. Allowed: InStock, Reserved, Sold");

        var items = await vinylRecordQueries.GetByStatusAsync(vinylStatus, cancellationToken);
        return items.Select(VinylRecordDto.FromDomainModel).ToList();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<VinylRecordDto>>> Search(
        [FromQuery] string? title = null,
        [FromQuery] int? releaseYear = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        CancellationToken cancellationToken = default)
    {
        var items = await vinylRecordQueries.GetAllAsync(cancellationToken);
        
        if (!string.IsNullOrEmpty(title))
            items = items.Where(vr => vr.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (releaseYear.HasValue)
            items = items.Where(vr => vr.ReleaseYear == releaseYear.Value).ToList();
        
        if (minPrice.HasValue)
            items = items.Where(vr => vr.Price >= minPrice.Value).ToList();
        
        if (maxPrice.HasValue)
            items = items.Where(vr => vr.Price <= maxPrice.Value).ToList();

        return items.Select(VinylRecordDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<VinylRecordDto>> Create([FromBody] CreateVinylRecordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cmd = new CreateVinylRecordCommand
            {
                Title = dto.Title,
                Genre = dto.Genre,
                ReleaseYear = dto.ReleaseYear,
                ArtistId = dto.ArtistId,
                LabelId = dto.LabelId,
                Price = dto.Price,
                Description = dto.Description
            };

            var created = await sender.Send(cmd, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, VinylRecordDto.FromDomainModel(created));
        }
        catch (DbUpdateException ex)
        {
            var isUniqueViolation = false;
            
            if (ex.InnerException is PostgresException pgEx)
            {
                isUniqueViolation = pgEx.SqlState == "23505";
            }
            else
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                isUniqueViolation = message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                                   message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
                                   message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
            }
            
            if (isUniqueViolation)
            {
                return Conflict("A vinyl record with this title and artist already exists.");
            }
            throw;
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VinylRecordDto>> Update(Guid id, [FromBody] UpdateVinylRecordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cmd = new UpdateVinylRecordCommand
            {
                Id = id,
                Title = dto.Title,
                Genre = dto.Genre,
                ReleaseYear = dto.ReleaseYear,
                Price = dto.Price,
                Description = dto.Description
            };

            var updated = await sender.Send(cmd, cancellationToken);
            return VinylRecordDto.FromDomainModel(updated);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            var isUniqueViolation = false;
            
            if (ex.InnerException is PostgresException pgEx)
            {
                isUniqueViolation = pgEx.SqlState == "23505";
            }
            else
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                isUniqueViolation = message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                                   message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
                                   message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
            }
            
            if (isUniqueViolation)
            {
                return Conflict("A vinyl record with this title and artist already exists.");
            }
            throw;
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeVinylRecordStatusDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.VinylRecords.VinylRecordStatus>(dto.Status, true, out var newStatus))
            return BadRequest("Invalid status value. Allowed: InStock, Reserved, Sold");

        var cmd = new ChangeVinylRecordStatusCommand
        {
            Id = id,
            Status = newStatus
        };

        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cmd = new DeleteVinylRecordCommand { Id = id };
            await sender.Send(cmd, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("sales", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(ex.Message);
        }
    }
}
