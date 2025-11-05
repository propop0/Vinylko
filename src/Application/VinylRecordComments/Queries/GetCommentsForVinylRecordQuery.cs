using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecordComments.Queries
{
    public record GetCommentsForVinylRecordQuery : IRequest<IReadOnlyList<VinylRecordComment>>
    {
        public required Guid VinylRecordId { get; init; }
    }

    public class GetCommentsForVinylRecordQueryHandler : IRequestHandler<GetCommentsForVinylRecordQuery, IReadOnlyList<VinylRecordComment>>
    {
        private readonly IVinylRecordCommentQueries _queries;

        public GetCommentsForVinylRecordQueryHandler(IVinylRecordCommentQueries queries)
        {
            _queries = queries;
        }

        public async Task<IReadOnlyList<VinylRecordComment>> Handle(GetCommentsForVinylRecordQuery request, CancellationToken cancellationToken)
        {
            return await _queries.GetByVinylRecordIdAsync(request.VinylRecordId, cancellationToken);
        }
    }
}


