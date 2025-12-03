using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Genres.Commands;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace Api.Controllers;

[Route("api/genres")]
[ApiController]
public class GenresController(IGenreQueries genreQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GenreDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await genreQueries.GetAllAsync(cancellationToken);
        return items.Select(GenreDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GenreDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var itemOption = await genreQueries.GetByIdAsync(id, cancellationToken);
        return itemOption.Match<ActionResult<GenreDto>>(
            some: genre => GenreDto.FromDomainModel(genre),
            none: () => NotFound());
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<GenreDto>> GetByName(string name, CancellationToken cancellationToken)
    {
        var itemOption = await genreQueries.GetByNameAsync(name, cancellationToken);
        return itemOption.Match<ActionResult<GenreDto>>(
            some: genre => GenreDto.FromDomainModel(genre),
            none: () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CreateGenreCommand
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<GenreDto>>(
            value => CreatedAtAction(nameof(GetById), new { id = value.Id }, GenreDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GenreDto>> Update(Guid id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var cmd = new UpdateGenreCommand
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<GenreDto>>(
            value => GenreDto.FromDomainModel(value),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteGenreCommand { Id = id };
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
