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
    public record CompleteSaleCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
        public string? Notes { get; init; }
    }

    public class CompleteSaleCommandHandler : IRequestHandler<CompleteSaleCommand, ErrorOr<Success>>
    {
        private readonly ISaleRepository _saleRepository;

        public CompleteSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<ErrorOr<Success>> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
        {
            var saleOption = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!saleOption.HasValue)
            {
                return Errors.Sale.NotFound(request.Id);
            }

            var sale = saleOption.ValueOr(() => throw new InvalidOperationException());
            
            // Перевірка статусу - можна завершити тільки Pending продаж
            if (sale.Status != SaleStatus.Pending)
            {
                return Errors.Sale.InvalidStatus(sale.Status.ToString(), SaleStatus.Completed.ToString());
            }

            sale.Complete(request.Notes);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            return Result.Success;
        }
    }
}


