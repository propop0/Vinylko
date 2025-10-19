using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.Sales;
using MediatR;

namespace Application.Sales.Commands
{
    public record CreateSaleCommand : IRequest<Sale>
    {
        public required string SaleNumber { get; init; }
        public required Guid RecordId { get; init; }
        public required decimal Price { get; init; }
        public required DateTime SaleDate { get; init; }
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
        public string? Notes { get; init; }
    }

    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Sale>
    {
        private readonly ISaleRepository _saleRepository;

        public CreateSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public Task<Sale> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = Sale.New(
                Guid.NewGuid(),
                request.SaleNumber,
                request.RecordId,
                request.Price,
                request.SaleDate,
                request.CustomerName,
                request.CustomerEmail);

            return _saleRepository.AddAsync(sale, cancellationToken);
        }
    }
}


