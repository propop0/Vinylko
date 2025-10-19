using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Sales;
using MediatR;

namespace Application.Sales.Queries
{
    public record GetSalesQuery : IRequest<IReadOnlyList<Sale>>;

    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, IReadOnlyList<Sale>>
    {
        private readonly ISaleQueries _queries;

        public GetSalesQueryHandler(ISaleQueries queries)
        {
            _queries = queries;
        }

        public Task<IReadOnlyList<Sale>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetAllAsync(cancellationToken);
        }
    }
}


