using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Genres.Commands
{
    public record DeleteGenreCommand : IRequest
    {
        public required Guid Id { get; init; }
    }

    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
    {
        private readonly IGenreRepository _genreRepository;

        public DeleteGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            // Check if genre exists
            var existing = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null)
            {
                throw new InvalidOperationException("Genre not found");
            }

            // Check if genre has vinyl records before attempting deletion
            var hasVinylRecords = await _genreRepository.HasVinylRecordsAsync(request.Id, cancellationToken);
            if (hasVinylRecords)
            {
                throw new InvalidOperationException("Cannot delete genre. There are vinyl records associated with this genre. Please delete or reassign the vinyl records first.");
            }

            await _genreRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


