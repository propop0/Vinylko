using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecordComments.Commands
{
    public record UpdateVinylRecordCommentCommand : IRequest<VinylRecordComment>
    {
        public required Guid Id { get; init; }
        public required string Content { get; init; }
    }

    public class UpdateVinylRecordCommentCommandHandler : IRequestHandler<UpdateVinylRecordCommentCommand, VinylRecordComment>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;

        public UpdateVinylRecordCommentCommandHandler(IVinylRecordCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<VinylRecordComment> Handle(UpdateVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            var existing = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null)
            {
                throw new InvalidOperationException("Comment not found");
            }

            existing.UpdateContent(request.Content);
            await _commentRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


