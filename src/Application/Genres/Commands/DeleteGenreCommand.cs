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
            await _genreRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


