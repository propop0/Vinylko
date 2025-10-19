using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.VinylRecords;
using MediatR;

namespace Application.VinylRecords.Queries
{
    public record GetVinylRecordByIdQuery(Guid Id) : IRequest<VinylRecord?>;

    public class GetVinylRecordByIdQueryHandler : IRequestHandler<GetVinylRecordByIdQuery, VinylRecord?>
    {
        private readonly IVinylRecordQueries _queries;

        public GetVinylRecordByIdQueryHandler(IVinylRecordQueries queries)
        {
            _queries = queries;
        }

        public Task<VinylRecord?> Handle(GetVinylRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


