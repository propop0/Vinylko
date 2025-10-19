using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Queries
{
    public record GetVinylRecordsQuery : IRequest<IReadOnlyList<VinylRecord>>;

    public class GetVinylRecordsQueryHandler : IRequestHandler<GetVinylRecordsQuery, IReadOnlyList<VinylRecord>>
    {
        private readonly IVinylRecordQueries _queries;

        public GetVinylRecordsQueryHandler(IVinylRecordQueries queries)
        {
            _queries = queries;
        }

        public Task<IReadOnlyList<VinylRecord>> Handle(GetVinylRecordsQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetAllAsync(cancellationToken);
        }
    }
}


