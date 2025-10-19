using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.VinylRecords.Commands
{
    public record DeleteVinylRecordCommand : IRequest
    {
        public required Guid Id { get; init; }
    }

    public class DeleteVinylRecordCommandHandler : IRequestHandler<DeleteVinylRecordCommand>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public DeleteVinylRecordCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task Handle(DeleteVinylRecordCommand request, CancellationToken cancellationToken)
        {
            await _vinylRecordRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


