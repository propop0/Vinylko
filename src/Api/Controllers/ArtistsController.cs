using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Artists.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/artists")]
[ApiController]
public class ArtistsController(IArtistQueries artistQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArtistDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await artistQueries.GetAllAsync(cancellationToken);
        return items.Select(ArtistDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArtistDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await artistQueries.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return ArtistDto.FromDomainModel(item);
    }

    [HttpGet("country/{country}")]
    public async Task<ActionResult<IReadOnlyList<ArtistDto>>> GetByCountry(string country, CancellationToken cancellationToken)
    {
        var items = await artistQueries.GetByCountryAsync(country, cancellationToken);
        return items.Select(ArtistDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<ArtistDto>> Create([FromBody] CreateArtistDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CreateArtistCommand
        {
            Name = dto.Name,
            Bio = dto.Bio,
            Country = dto.Country,
            BirthDate = dto.BirthDate,
            Website = dto.Website
        };

        var created = await sender.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ArtistDto.FromDomainModel(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ArtistDto>> Update(Guid id, [FromBody] UpdateArtistDto dto, CancellationToken cancellationToken)
    {
        var cmd = new UpdateArtistCommand
        {
            Id = id,
            Name = dto.Name,
            Bio = dto.Bio,
            Country = dto.Country,
            BirthDate = dto.BirthDate,
            Website = dto.Website
        };

        var updated = await sender.Send(cmd, cancellationToken);
        return ArtistDto.FromDomainModel(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteArtistCommand { Id = id };
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }
}
