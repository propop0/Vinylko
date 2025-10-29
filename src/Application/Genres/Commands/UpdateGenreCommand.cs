using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.Genres;
using MediatR;

namespace Application.Genres.Commands
{
    public record UpdateGenreCommand : IRequest<Genre>
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Genre>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<Genre> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var existing = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new InvalidOperationException("Genre not found");

            existing.UpdateDetails(request.Name, request.Description);
            await _genreRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


