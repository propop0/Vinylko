using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Sales.Commands
{
    public record CancelSaleCommand : IRequest
    {
        public required Guid Id { get; init; }
    }

    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale is null) return;

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }
    }
}


