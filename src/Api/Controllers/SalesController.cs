using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Sales.Commands;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace Api.Controllers;

[Route("api/sales")]
[ApiController]
public class SalesController(ISaleQueries saleQueries, ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await saleQueries.GetAllAsync(cancellationToken);
        return items.Select(SaleDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var itemOption = await saleQueries.GetByIdAsync(id, cancellationToken);
        return itemOption.Match<ActionResult<SaleDto>>(
            some: sale => SaleDto.FromDomainModel(sale),
            none: () => NotFound());
    }

    [HttpGet("record/{recordId:guid}")]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetByRecord(Guid recordId, CancellationToken cancellationToken)
    {
        var items = await saleQueries.GetByRecordIdAsync(recordId, cancellationToken);
        return items.Select(SaleDto.FromDomainModel).ToList();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetByStatus(string status, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Domain.Sales.SaleStatus>(status, true, out var saleStatus))
            return BadRequest("Invalid status value. Allowed: Pending, Completed, Cancelled");

        var items = await saleQueries.GetByStatusAsync(saleStatus, cancellationToken);
        return items.Select(SaleDto.FromDomainModel).ToList();
    }

    [HttpGet("customer/{email}")]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetByCustomer(string email, CancellationToken cancellationToken)
    {
        var items = await saleQueries.GetByCustomerEmailAsync(email, cancellationToken);
        return items.Select(SaleDto.FromDomainModel).ToList();
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        var items = await saleQueries.GetByDateRangeAsync(startDate, endDate, cancellationToken);
        return items.Select(SaleDto.FromDomainModel).ToList();
    }

    [HttpGet("total-amount")]
    public async Task<ActionResult<decimal>> GetTotalAmount(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var total = await saleQueries.GetTotalSalesAmountAsync(startDate, endDate, cancellationToken);
        return total;
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create([FromBody] CreateSaleDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CreateSaleCommand
        {
            SaleNumber = $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
            RecordId = dto.RecordId,
            Price = dto.Price,
            SaleDate = dto.SaleDate,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<ActionResult<SaleDto>>(
            value => CreatedAtAction(nameof(GetById), new { id = value.Id }, SaleDto.FromDomainModel(value)),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                ErrorType.Conflict => Conflict(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteSaleDto dto, CancellationToken cancellationToken)
    {
        var cmd = new CompleteSaleCommand
        {
            Id = id,
            Notes = dto.Notes
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                ErrorType.Validation => BadRequest(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var cmd = new CancelSaleCommand { Id = id };
        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                ErrorType.Validation => BadRequest(errors),
                _ => BadRequest(errors)
            });
    }

    [HttpPatch("{id:guid}/customer")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateSaleCustomerDto dto, CancellationToken cancellationToken)
    {
        var cmd = new UpdateSaleCustomerCommand
        {
            Id = id,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail
        };

        var result = await sender.Send(cmd, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => errors.First().Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => BadRequest(errors)
            });
    }
}
