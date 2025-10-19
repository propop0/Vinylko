using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Sales.Commands
{
    public record CompleteSaleCommand : IRequest
    {
        public required Guid Id { get; init; }
        public string? Notes { get; init; }
    }

    public class CompleteSaleCommandHandler : IRequestHandler<CompleteSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public CompleteSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale is null) return;

            sale.Complete(request.Notes);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }
    }
}


