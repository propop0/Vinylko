using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Sales;
using MediatR;

namespace Application.Sales.Queries
{
    public record GetSaleByIdQuery(Guid Id) : IRequest<Sale?>;

    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Sale?>
    {
        private readonly ISaleQueries _queries;

        public GetSaleByIdQueryHandler(ISaleQueries queries)
        {
            _queries = queries;
        }

        public Task<Sale?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


