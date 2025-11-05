using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecordComments.Commands
{
    public record CreateVinylRecordCommentCommand : IRequest<VinylRecordComment>
    {
        public required Guid VinylRecordId { get; init; }
        public required string Content { get; init; }
    }

    public class CreateVinylRecordCommentCommandHandler : IRequestHandler<CreateVinylRecordCommentCommand, VinylRecordComment>
    {
        private readonly IVinylRecordCommentRepository _commentRepository;

        public CreateVinylRecordCommentCommandHandler(IVinylRecordCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<VinylRecordComment> Handle(CreateVinylRecordCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = VinylRecordComment.New(Guid.NewGuid(), request.VinylRecordId, request.Content);
            return await _commentRepository.AddAsync(comment, cancellationToken);
        }
    }
}


