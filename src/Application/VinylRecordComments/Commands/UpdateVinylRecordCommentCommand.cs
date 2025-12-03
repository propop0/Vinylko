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
    public record UpdateVinylRecordCommentCommand : IRequest<ErrorOr<VinylRecordComment>>
    {
        public required Guid Id { get; init; }
        public required string Content { get; init; }
    }

    public class UpdateVinylRecordCommentCommandHandler : IRequestHandler<UpdateVinylRecordCommentCommand, ErrorOr<VinylRecordComment>>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;

        public UpdateVinylRecordCommentCommandHandler(IVinylRecordCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<ErrorOr<VinylRecordComment>> Handle(UpdateVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!existingOption.HasValue)
            {
                return Errors.VinylRecordComment.NotFound(request.Id);
            }

            var existing = existingOption.ValueOr(() => throw new InvalidOperationException());
            existing.UpdateContent(request.Content);
            await _commentRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


