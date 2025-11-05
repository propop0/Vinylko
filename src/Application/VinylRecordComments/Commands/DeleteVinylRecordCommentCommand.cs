using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.VinylRecordComments.Commands
{
    public record DeleteVinylRecordCommentCommand : IRequest
    {
        public required Guid Id { get; init; }
    }

    public class DeleteVinylRecordCommentCommandHandler : IRequestHandler<DeleteVinylRecordCommentCommand>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;

        public DeleteVinylRecordCommentCommandHandler(IVinylRecordCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(DeleteVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            await _commentRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}


