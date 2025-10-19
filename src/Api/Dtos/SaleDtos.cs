using Domain.Sales;

namespace Api.Dtos;

public record SaleDto(
    Guid Id,
    string SaleNumber,
    Guid RecordId,
    decimal Price,
    string Status,
    DateTime SaleDate,
    string? Notes,
    string? CustomerName,
    string? CustomerEmail,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static SaleDto FromDomainModel(Sale sale)
        => new(
            sale.Id,
            sale.SaleNumber,
            sale.RecordId,
            sale.Price,
            sale.Status.ToString(),
            sale.SaleDate,
            sale.Notes,
            sale.CustomerName,
            sale.CustomerEmail,
            sale.CreatedAt,
            sale.UpdatedAt);
}

public record CreateSaleDto(
    Guid RecordId,
    decimal Price,
    DateTime SaleDate,
    string? CustomerName = null,
    string? CustomerEmail = null);

public record CompleteSaleDto(string? Notes = null);

public record UpdateSaleCustomerDto(
    string? CustomerName = null,
    string? CustomerEmail = null);
