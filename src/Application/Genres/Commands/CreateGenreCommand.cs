using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Genres;
using ErrorOr;
using MediatR;

namespace Application.Genres.Commands
{
    public record CreateGenreCommand : IRequest<ErrorOr<Genre>>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, ErrorOr<Genre>>
    {
        private readonly IGenreRepository _genreRepository;

        public CreateGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ErrorOr<Genre>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var exists = await _genreRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (exists)
            {
                return Errors.Genre.AlreadyExists(request.Name);
            }

            var genre = Genre.New(Guid.NewGuid(), request.Name, request.Description);
            return await _genreRepository.AddAsync(genre, cancellationToken);
        }
    }
}


