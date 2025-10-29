using Domain.Sales;

namespace Tests.Data.Sales;

public static class SaleData
{
    public static Sale FirstSale(Guid recordId)
    {
        return Sale.New(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            saleNumber: "SALE-2024-0001",
            recordId: recordId,
            price: 29.99m,
            saleDate: new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
            customerName: "John Doe",
            customerEmail: "john.doe@example.com"
        );
    }

    public static Sale SecondSale(Guid recordId)
    {
        return Sale.New(
            id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            saleNumber: "SALE-2024-0002",
            recordId: recordId,
            price: 34.99m,
            saleDate: new DateTime(2024, 2, 20, 14, 30, 0, DateTimeKind.Utc),
            customerName: "Jane Smith",
            customerEmail: "jane.smith@example.com"
        );
    }

    public static Sale ThirdSale(Guid recordId)
    {
        return Sale.New(
            id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            saleNumber: "SALE-2024-0003",
            recordId: recordId,
            price: 32.50m,
            saleDate: new DateTime(2024, 3, 10, 16, 45, 0, DateTimeKind.Utc),
            customerName: "Bob Johnson",
            customerEmail: "bob.johnson@example.com"
        );
    }

    public static Sale CreateSaleWithCustomCustomer(string customerName, string customerEmail, Guid recordId)
    {
        return Sale.New(
            id: Guid.NewGuid(),
            saleNumber: $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
            recordId: recordId,
            price: 25.00m,
            saleDate: DateTime.UtcNow,
            customerName: customerName,
            customerEmail: customerEmail
        );
    }
}

