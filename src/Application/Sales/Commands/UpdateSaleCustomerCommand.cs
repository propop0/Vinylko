using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Optional;

namespace Application.Sales.Commands
{
    public record UpdateSaleCustomerCommand : IRequest<ErrorOr<Success>>
    {
        public required Guid Id { get; init; }
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
    }

    public class UpdateSaleCustomerCommandHandler : IRequestHandler<UpdateSaleCustomerCommand, ErrorOr<Success>>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleCustomerCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateSaleCustomerCommand request, CancellationToken cancellationToken)
        {
            var saleOption = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!saleOption.HasValue)
            {
                return Errors.Sale.NotFound(request.Id);
            }

            var sale = saleOption.ValueOr(() => throw new InvalidOperationException());
            sale.UpdateCustomerInfo(request.CustomerName, request.CustomerEmail);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            return Result.Success;
        }
    }
}
