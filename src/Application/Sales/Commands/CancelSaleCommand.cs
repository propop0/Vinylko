using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Sales;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Sales.Commands
{
    public record CancelSaleCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
    }

    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, ErrorOr<Success>>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<ErrorOr<Success>> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var saleOption = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!saleOption.HasValue)
            {
                return Errors.Sale.NotFound(request.Id);
            }

            var sale = saleOption.ValueOr(() => throw new InvalidOperationException());
            
            // Перевірка статусу - можна скасувати тільки Pending або Completed продаж
            if (sale.Status == SaleStatus.Cancelled)
            {
                return Errors.Sale.InvalidStatus(sale.Status.ToString(), SaleStatus.Cancelled.ToString());
            }

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            return Result.Success;
        }
    }
}


