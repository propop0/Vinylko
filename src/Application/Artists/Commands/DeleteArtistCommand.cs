using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Artists.Commands
{
    public record DeleteArtistCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
    }

    public class DeleteArtistCommandHandler : IRequestHandler<DeleteArtistCommand, ErrorOr<Success>>
    {
        private readonly IArtistRepository _artistRepository;

        public DeleteArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _artistRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!existingOption.HasValue)
            {
                return Errors.Artist.NotFound(request.Id);
            }

            var hasVinylRecords = await _artistRepository.HasVinylRecordsAsync(request.Id, cancellationToken);
            if (hasVinylRecords)
            {
                return Errors.Artist.HasVinylRecords(request.Id);
            }

            await _artistRepository.DeleteAsync(request.Id, cancellationToken);
            return Result.Success;
        }
    }
}


