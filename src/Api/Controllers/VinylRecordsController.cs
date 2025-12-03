using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.VinylRecords.Commands;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

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
        var itemOption = await vinylRecordQueries.GetByIdAsync(id, cancellationToken);
        return itemOption.Match<ActionResult<VinylRecordDto>>(
            some: vinyl => VinylRecordDto.FromDomainModel(vinyl),
            none: () => NotFound());
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
        var cmd = new CreateVinylRecordCommand
        {
            Title = dto.Title,
            Genre = dto.Genre,
            ReleaseYear = dto.ReleaseYear,
            ArtistId = dto.ArtistId,
            Price = dto.Price,
            Description = dto.Description
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<VinylRecordDto>>(
            value => CreatedAtAction(nameof(GetById), new { id = value.Id }, VinylRecordDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VinylRecordDto>> Update(Guid id, [FromBody] UpdateVinylRecordDto dto, CancellationToken cancellationToken)
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

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<VinylRecordDto>>(
            value => VinylRecordDto.FromDomainModel(value),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
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

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteVinylRecordCommand { Id = id };
        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }
}
