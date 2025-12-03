using Api.Dtos;
using Application.ArtistGenres.Commands;
using Application.ArtistGenres.Queries;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/artist-genres")]
[ApiController]
public class ArtistGenresController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ArtistGenreDto>> AddGenreToArtist(
        [FromBody] AddGenreToArtistDto dto,
        CancellationToken cancellationToken)
    {
        var cmd = new AddGenreToArtistCommand
        {
            ArtistId = dto.ArtistId,
            GenreId = dto.GenreId
        };

        var result = await sender.Send(cmd, cancellationToken);
        
        return result.Match<ActionResult<ArtistGenreDto>>(
            value => CreatedAtAction(
                nameof(GetArtistGenres),
                new { artistId = value.ArtistId },
                ArtistGenreDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(errors),
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpGet("artist/{artistId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ArtistGenreDto>>> GetArtistGenres(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        var query = new GetArtistGenresQuery { ArtistId = artistId };
        var items = await sender.Send(query, cancellationToken);
        return items.Select(ArtistGenreDto.FromDomainModel).ToList();
    }

    [HttpDelete("artist/{artistId:guid}/genre/{genreId:guid}")]
    public async Task<IActionResult> RemoveGenreFromArtist(
        Guid artistId,
        Guid genreId,
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveGenreFromArtistCommand
        {
            ArtistId = artistId,
            GenreId = genreId
        };

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

