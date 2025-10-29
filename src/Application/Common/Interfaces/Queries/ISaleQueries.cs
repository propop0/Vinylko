using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Sales;

namespace Application.Common.Interfaces.Queries;

public interface ISaleQueries
{
    Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetByRecordIdAsync(Guid recordId, CancellationToken cancellationToken);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetByStatusAsync(SaleStatus status, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetByCustomerEmailAsync(string customerEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<decimal> GetTotalSalesAmountAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}


