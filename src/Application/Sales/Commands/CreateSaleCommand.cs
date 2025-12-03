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
    public record CreateSaleCommand : IRequest<ErrorOr<Sale>>
    {
        public required string SaleNumber { get; init; }
        public required Guid RecordId { get; init; }
        public required decimal Price { get; init; }
        public required DateTime SaleDate { get; init; }
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
        public string? Notes { get; init; }
    }

    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, ErrorOr<Sale>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IVinylRecordRepository _vinylRecordRepository;
        private readonly IArtistRepository _artistRepository;

        public CreateSaleCommandHandler(
            ISaleRepository saleRepository,
            IVinylRecordRepository vinylRecordRepository,
            IArtistRepository artistRepository)
        {
            _saleRepository = saleRepository;
            _vinylRecordRepository = vinylRecordRepository;
            _artistRepository = artistRepository;
        }

        public async Task<ErrorOr<Sale>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Перевірка чи існує платівка
            var vinylRecordOption = await _vinylRecordRepository.GetByIdAsync(request.RecordId, cancellationToken);
            if (!vinylRecordOption.HasValue)
            {
                return Errors.VinylRecord.NotFound(request.RecordId);
            }

            var vinylRecord = vinylRecordOption.ValueOr(() => throw new InvalidOperationException());

            // Отримуємо ім'я артиста для знімка
            string? artistName = null;
            var artistOption = await _artistRepository.GetByIdAsync(vinylRecord.ArtistId, cancellationToken);
            if (artistOption.HasValue)
            {
                artistName = artistOption.ValueOr(() => throw new InvalidOperationException()).Name;
            }

            // Перевірка чи sale number вже існує
            var saleNumberExists = await _saleRepository.ExistsBySaleNumberAsync(request.SaleNumber, cancellationToken);
            if (saleNumberExists)
            {
                return Errors.Sale.SaleNumberAlreadyExists(request.SaleNumber);
            }

            // Створюємо продаж зі знімком назви платівки та імені артиста
            var sale = Sale.New(
                Guid.NewGuid(),
                request.SaleNumber,
                request.RecordId,
                request.Price,
                request.SaleDate,
                recordTitle: vinylRecord.Title,
                artistName: artistName,
                request.CustomerName,
                request.CustomerEmail);

            return await _saleRepository.AddAsync(sale, cancellationToken);
        }
    }
}


