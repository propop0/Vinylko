using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Genres.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        var item = await genreQueries.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return GenreDto.FromDomainModel(item);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<GenreDto>> GetByName(string name, CancellationToken cancellationToken)
    {
        var item = await genreQueries.GetByNameAsync(name, cancellationToken);
        if (item is null) return NotFound();
        return GenreDto.FromDomainModel(item);
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CreateGenreCommand
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var created = await sender.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, GenreDto.FromDomainModel(created));
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

        var updated = await sender.Send(cmd, cancellationToken);
        return GenreDto.FromDomainModel(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new DeleteGenreCommand { Id = id };
        await sender.Send(cmd, cancellationToken);
        return NoContent();
    }
}
