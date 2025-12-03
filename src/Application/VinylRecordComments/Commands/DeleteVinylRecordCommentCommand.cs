using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.VinylRecordComments.Commands
{
    public record DeleteVinylRecordCommentCommand : IRequest<ErrorOr<Deleted>>
    {
        public required Guid Id { get; init; }
    }

    public class DeleteVinylRecordCommentCommandHandler : IRequestHandler<DeleteVinylRecordCommentCommand, ErrorOr<Deleted>>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;

        public DeleteVinylRecordCommentCommandHandler(IVinylRecordCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            var existingOption = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!existingOption.HasValue)
            {
                return Errors.VinylRecordComment.NotFound(request.Id);
            }

            await _commentRepository.DeleteAsync(request.Id, cancellationToken);
            return Result.Deleted;
        }
    }
}


