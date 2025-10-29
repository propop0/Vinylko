using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Sales.Commands
{
    public record UpdateSaleCustomerCommand : IRequest
    {
        public required Guid Id { get; init; }
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
    }

    public class UpdateSaleCustomerCommandHandler : IRequestHandler<UpdateSaleCustomerCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleCustomerCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(UpdateSaleCustomerCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale is null) return;

            sale.UpdateCustomerInfo(request.CustomerName, request.CustomerEmail);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }
    }
}
