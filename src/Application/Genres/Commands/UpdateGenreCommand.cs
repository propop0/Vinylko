using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Genres.Commands
{
    public record UpdateGenreCommand : IRequest
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var existing = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return;

            existing.UpdateDetails(request.Name, request.Description);
            await _genreRepository.UpdateAsync(existing, cancellationToken);
        }
    }
}


