using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Commands
{
    public record ChangeVinylRecordStatusCommand : IRequest
    {
        public required Guid Id { get; init; }
        public required VinylRecordStatus Status { get; init; }
    }

    public class ChangeVinylRecordStatusCommandHandler : IRequestHandler<ChangeVinylRecordStatusCommand>
    {
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public ChangeVinylRecordStatusCommandHandler(IVinylRecordRepository vinylRecordRepository)
        {
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task Handle(ChangeVinylRecordStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _vinylRecordRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) return;

            entity.ChangeStatus(request.Status);
            await _vinylRecordRepository.UpdateAsync(entity, cancellationToken);
        }
    }
}


