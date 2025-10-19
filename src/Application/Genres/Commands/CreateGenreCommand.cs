using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.Genres;
using MediatR;

namespace Application.Genres.Commands
{
    public record CreateGenreCommand : IRequest<Genre>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Genre>
    {
        private readonly IGenreRepository _genreRepository;

        public CreateGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<Genre> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = Genre.New(Guid.NewGuid(), request.Name, request.Description);
            return await _genreRepository.AddAsync(genre, cancellationToken);
        }
    }
}


