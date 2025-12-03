using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Sales;
using MediatR;
using Optional;

namespace Application.Sales.Queries
{
    public record GetSaleByIdQuery(Guid Id) : IRequest<Option<Sale>>;

    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Option<Sale>>
    {
        private readonly ISaleQueries _queries;

        public GetSaleByIdQueryHandler(ISaleQueries queries)
        {
            _queries = queries;
        }

        public Task<Option<Sale>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


