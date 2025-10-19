namespace Domain.Sales;

public class Sale
{
    public Guid Id { get; }
    public string SaleNumber { get; private set; }
    public Guid RecordId { get; private set; }
    public decimal Price { get; private set; }
    public SaleStatus Status { get; private set; }
    public DateTime SaleDate { get; private set; }
    public string? Notes { get; private set; }
    public string? CustomerName { get; private set; }
    public string? CustomerEmail { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    // приватний конструктор
    private Sale(Guid id,
        string saleNumber,
        Guid recordId,
        decimal price,
        SaleStatus status,
        DateTime saleDate,
        string? notes,
        string? customerName,
        string? customerEmail,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        SaleNumber = saleNumber;
        RecordId = recordId;
        Price = price;
        Status = status;
        SaleDate = saleDate;
        Notes = notes;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Sale New(
        Guid id,
        string saleNumber,
        Guid recordId,
        decimal price,
        DateTime saleDate,
        string? customerName = null,
        string? customerEmail = null)
    {
        return new Sale(
            id,
            saleNumber,
            recordId,
            price,
            SaleStatus.Pending,
            saleDate,
            null,
            customerName,
            customerEmail,
            DateTime.UtcNow,
            null);
    }

    public void Complete(string? notes = null)
    {
        Status = SaleStatus.Completed;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCustomerInfo(string? customerName, string? customerEmail)
    {
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum SaleStatus
{
    Pending,
    Completed,
    Cancelled
}
