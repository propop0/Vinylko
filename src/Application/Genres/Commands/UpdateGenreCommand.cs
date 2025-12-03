using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Genres;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Genres.Commands
{
    public record UpdateGenreCommand : IRequest<ErrorOr<Genre>>
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, ErrorOr<Genre>>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ErrorOr<Genre>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (!existingOption.HasValue)
            {
                return Errors.Genre.NotFound(request.Id);
            }

            var genre = existingOption.ValueOr(() => throw new InvalidOperationException());
            genre.UpdateDetails(request.Name, request.Description);
            await _genreRepository.UpdateAsync(genre, cancellationToken);
            return genre;
        }
    }
}
