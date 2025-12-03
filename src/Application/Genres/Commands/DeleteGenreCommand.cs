using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Genres.Commands
{
    public record DeleteGenreCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
    }

    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, ErrorOr<Success>>
    {
        private readonly IGenreRepository _genreRepository;

        public DeleteGenreCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!existingOption.HasValue)
            {
                return Errors.Genre.NotFound(request.Id);
            }

            var hasVinylRecords = await _genreRepository.HasVinylRecordsAsync(request.Id, cancellationToken);
            if (hasVinylRecords)
            {
                return Errors.Genre.HasVinylRecords(request.Id);
            }

            await _genreRepository.DeleteAsync(request.Id, cancellationToken);
            return Result.Success;
        }
    }
}
