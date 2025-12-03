using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.VinylRecordComments.Commands
{
    public record CreateVinylRecordCommentCommand : IRequest<ErrorOr<VinylRecordComment>>
    {
        public required Guid VinylRecordId { get; init; }
        public required string Content { get; init; }
    }

    public class CreateVinylRecordCommentCommandHandler : IRequestHandler<CreateVinylRecordCommentCommand, ErrorOr<VinylRecordComment>>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;
        private readonly IVinylRecordRepository _vinylRecordRepository;

        public CreateVinylRecordCommentCommandHandler(
            IVinylRecordCommentRepository commentRepository,
            IVinylRecordRepository vinylRecordRepository)
        {
            _commentRepository = commentRepository;
            _vinylRecordRepository = vinylRecordRepository;
        }

        public async Task<ErrorOr<VinylRecordComment>> Handle(CreateVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            // Перевірка чи існує платівка
            var vinylRecordOption = await _vinylRecordRepository.GetByIdAsync(request.VinylRecordId, cancellationToken);
            if (!vinylRecordOption.HasValue)
            {
                return Errors.VinylRecordComment.VinylRecordNotFound(request.VinylRecordId);
            }

            var comment = VinylRecordComment.New(Guid.NewGuid(), request.VinylRecordId, request.Content);
            return await _commentRepository.AddAsync(comment, cancellationToken);
        }
    }
}


