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
            // Check if vinyl record has sales before attempting deletion
            var hasSales = await _vinylRecordRepository.HasSalesAsync(request.Id, cancellationToken);
            if (hasSales)
            {
                throw new InvalidOperationException("Cannot delete vinyl record. There are sales associated with this record. Sales history must be preserved.");
            }

            await _vinylRecordRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


