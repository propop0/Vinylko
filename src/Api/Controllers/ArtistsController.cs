using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Artists.Commands;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

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
        var itemOption = await artistQueries.GetByIdAsync(id, cancellationToken);
        return itemOption.Match<ActionResult<ArtistDto>>(
            some: artist => ArtistDto.FromDomainModel(artist),
            none: () => NotFound());
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

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<ArtistDto>>(
            value => CreatedAtAction(nameof(GetById), new { id = value.Id }, ArtistDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
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

        var result = await sender.Send(cmd, cancellationToken);
        
        return result.Match<ActionResult<ArtistDto>>(
            value => ArtistDto.FromDomainModel(value),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteArtistCommand { Id = id };
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
