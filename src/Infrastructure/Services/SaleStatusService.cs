using Domain.Sales;

namespace Infrastructure.Services;

public interface ISaleStatusService
{
    bool CanChangeStatus(SaleStatus currentStatus, SaleStatus newStatus);
    SaleStatus GetNextAvailableStatus(SaleStatus currentStatus);
}

public class SaleStatusService : ISaleStatusService
{
    public bool CanChangeStatus(SaleStatus currentStatus, SaleStatus newStatus)
    {
        return currentStatus switch
        {
            SaleStatus.Pending => newStatus == SaleStatus.Completed || newStatus == SaleStatus.Cancelled,
            SaleStatus.Completed => false, // Completed sales cannot change status
            SaleStatus.Cancelled => false, // Cancelled sales cannot change status
            _ => false
        };
    }

    public SaleStatus GetNextAvailableStatus(SaleStatus currentStatus)
    {
        return currentStatus switch
        {
            SaleStatus.Pending => SaleStatus.Completed,
            SaleStatus.Completed => SaleStatus.Completed, // Already completed
            SaleStatus.Cancelled => SaleStatus.Cancelled, // Already cancelled
            _ => SaleStatus.Pending
        };
    }
}
