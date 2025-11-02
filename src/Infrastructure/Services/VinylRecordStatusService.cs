using Domain.VinylRecords;

namespace Infrastructure.Services;

public interface IVinylRecordStatusService
{
    bool CanChangeStatus(VinylRecordStatus currentStatus, VinylRecordStatus newStatus);
    VinylRecordStatus GetNextAvailableStatus(VinylRecordStatus currentStatus);
}

public class VinylRecordStatusService : IVinylRecordStatusService
{
    public bool CanChangeStatus(VinylRecordStatus currentStatus, VinylRecordStatus newStatus)
    {
        return currentStatus switch
        {
            VinylRecordStatus.InStock => newStatus == VinylRecordStatus.Reserved || newStatus == VinylRecordStatus.Sold,
            VinylRecordStatus.Reserved => newStatus == VinylRecordStatus.InStock || newStatus == VinylRecordStatus.Sold,
            VinylRecordStatus.Sold => false,
            _ => false
        };
    }

    public VinylRecordStatus GetNextAvailableStatus(VinylRecordStatus currentStatus)
    {
        return currentStatus switch
        {
            VinylRecordStatus.InStock => VinylRecordStatus.Reserved,
            VinylRecordStatus.Reserved => VinylRecordStatus.Sold,
            VinylRecordStatus.Sold => VinylRecordStatus.Sold, 
            _ => VinylRecordStatus.InStock
        };
    }
}
