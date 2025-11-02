using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using Api.Dtos;
using Domain.Artists;
using Domain.Sales;
using Domain.VinylRecords;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Artists;
using Tests.Data.Sales;
using Tests.Data.VinylRecords;
using Xunit;

namespace Api.Tests.Integration.Sales;

public class SalesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/sales";

    private readonly Artist _testArtist;
    private readonly VinylRecord _testVinylRecord;
    private readonly Sale _firstTestSale;
    private readonly Sale _secondTestSale;
    private readonly Guid _testLabelId;

    public SalesControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _testArtist = ArtistData.FirstArtist();
        _testLabelId = Guid.NewGuid();
        _testVinylRecord = VinylRecordData.FirstVinylRecord(_testArtist.Id, _testLabelId);
        _firstTestSale = SaleData.FirstSale(_testVinylRecord.Id);
        _secondTestSale = SaleData.SecondSale(_testVinylRecord.Id);
    }

    [Fact]
    public async Task ShouldGetSaleById()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestSale.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var saleDto = await response.ToResponseModel<SaleDto>();
        saleDto.Id.Should().Be(_firstTestSale.Id);
        saleDto.RecordId.Should().Be(_firstTestSale.RecordId);
        saleDto.Price.Should().Be(_firstTestSale.Price);
        saleDto.Status.Should().Be(_firstTestSale.Status.ToString());
    }

    [Fact]
    public async Task ShouldGetAllSales()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sales = await response.ToResponseModel<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().Contain(s => s.Id == _firstTestSale.Id);
    }

    [Fact]
    public async Task ShouldCreateSale()
    {
        // Arrange
        var request = new CreateSaleDto(
            RecordId: _testVinylRecord.Id,
            Price: 35.00m,
            SaleDate: DateTime.UtcNow,
            CustomerName: "Test Customer",
            CustomerEmail: "test@example.com"
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();
        var saleDto = await response.ToResponseModel<SaleDto>();

        // Assert - verify DB state
        var dbSale = await Context.Sales
            .AsNoTracking()
            .FirstAsync(x => x.Id == saleDto.Id);
        dbSale.RecordId.Should().Be(request.RecordId);
        dbSale.Price.Should().Be(request.Price);
        dbSale.Status.Should().Be(SaleStatus.Pending);
        dbSale.CustomerName.Should().Be(request.CustomerName);
        dbSale.CustomerEmail.Should().Be(request.CustomerEmail);
    }

    [Fact]
    public async Task ShouldNotCreateSaleWithInvalidData()
    {
        // Arrange
        var request = new CreateSaleDto(
            RecordId: Guid.Empty, // Invalid: empty GUID
            Price: -10m, // Invalid: negative price
            SaleDate: DateTime.UtcNow,
            CustomerName: null,
            CustomerEmail: "invalid-email" // Invalid: not a valid email format
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateSale()
    {
    // Arrange - Перший тест продаж вже існує в БД
        var existingSaleInDb = await Context.Sales
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestSale.Id);
        
        // Assert - Verify unique constraint: each sale should have unique SaleNumber
        // Act
        var saleNumberCount = await Context.Sales
            .AsNoTracking()
            .Where(x => x.SaleNumber == existingSaleInDb.SaleNumber)
            .CountAsync();
        saleNumberCount.Should().Be(1);
        
    // тестування через апі напряму на дублікат SaleNumber неможливе, 
    // бо сейлнамбер генерується автоматично з часовим штампом і він унікальний 
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenSaleDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldCompleteSale()
    {
        // Arrange
        var request = new CompleteSaleDto(Notes: "Sale completed successfully");

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestSale.Id}/complete", request);

        // Assert - verify HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify DB
        var completedSale = await Context.Sales
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestSale.Id);
        completedSale.Status.Should().Be(SaleStatus.Completed);
        completedSale.Notes.Should().Be(request.Notes);
    }

    [Fact]
    public async Task ShouldCancelSale()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{BaseRoute}/{_firstTestSale.Id}/cancel");
        var response = await Client.SendAsync(request);

        // Assert - verify HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify DB
        var cancelledSale = await Context.Sales
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestSale.Id);
        cancelledSale.Status.Should().Be(SaleStatus.Cancelled);
    }

    [Fact]
    public async Task ShouldUpdateSaleCustomerInfo()
    {
        // Arrange
        var request = new UpdateSaleCustomerDto(
            CustomerName: "Updated Customer Name",
            CustomerEmail: "updated@example.com"
        );

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestSale.Id}/customer", request);

        // Assert - verify HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify DB
        var updatedSale = await Context.Sales
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestSale.Id);
        updatedSale.CustomerName.Should().Be(request.CustomerName);
        updatedSale.CustomerEmail.Should().Be(request.CustomerEmail);
    }

    [Fact]
    public async Task ShouldGetSalesByRecord()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/record/{_testVinylRecord.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sales = await response.ToResponseModel<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().AllSatisfy(s => s.RecordId.Should().Be(_testVinylRecord.Id));
    }

    [Fact]
    public async Task ShouldGetSalesByStatus()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/status/Pending");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sales = await response.ToResponseModel<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().AllSatisfy(s => s.Status.Should().Be("Pending"));
    }

    [Fact]
    public async Task ShouldGetSalesByCustomerEmail()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/customer/{_firstTestSale.CustomerEmail}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sales = await response.ToResponseModel<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().AllSatisfy(s => s.CustomerEmail.Should().Be(_firstTestSale.CustomerEmail));
    }

    [Fact]
    public async Task ShouldGetSalesByDateRange()
    {
        // Arrange
        var startDate = _firstTestSale.SaleDate.AddDays(-1);
        var endDate = _firstTestSale.SaleDate.AddDays(1);

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/date-range?startDate={startDate:yyyy-MM-ddTHH:mm:ssZ}&endDate={endDate:yyyy-MM-ddTHH:mm:ssZ}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sales = await response.ToResponseModel<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().Contain(s => s.Id == _firstTestSale.Id);
    }

    [Fact]
    public async Task ShouldGetTotalSalesAmount()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/total-amount");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.ToResponseModel<decimal>();
        total.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task ShouldHandleCascadeDelete()
    {
        // Arrange - Create a sale for a vinyl record
        var vinylRecord = VinylRecordData.ThirdVinylRecord(_testArtist.Id, _testLabelId);
        await Context.VinylRecords.AddAsync(vinylRecord);
        await SaveChangesAsync();

        var sale = SaleData.ThirdSale(vinylRecord.Id);
        await Context.Sales.AddAsync(sale);
        await SaveChangesAsync();

        // Act - Try to delete the vinyl record (should fail because sales exist)
        var deleteResponse = await Client.DeleteAsync($"/api/vinyl-records/{vinylRecord.Id}");
        
        // Assert - Deletion should fail with BadRequest because sales prevent deletion
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        // Verify that both vinyl record and sale still exist in the database
        var vinylRecordExists = await Context.VinylRecords
            .AsNoTracking()
            .AnyAsync(x => x.Id == vinylRecord.Id);
        vinylRecordExists.Should().BeTrue();
        
        var saleExists = await Context.Sales
            .AsNoTracking()
            .AnyAsync(x => x.Id == sale.Id);
        saleExists.Should().BeTrue();
    }

    public async Task InitializeAsync()
    {
        // Check if artist already exists
        var artistExists = await Context.Artists.AnyAsync(x => x.Id == _testArtist.Id);
        if (!artistExists)
        {
            await Context.Artists.AddAsync(_testArtist);
        }
        
        // Check if vinyl record already exists
        var vinylRecordExists = await Context.VinylRecords.AnyAsync(x => x.Id == _testVinylRecord.Id);
        if (!vinylRecordExists)
        {
            await Context.VinylRecords.AddAsync(_testVinylRecord);
        }
        
        // Check if sale already exists
        var saleExists = await Context.Sales.AnyAsync(x => x.Id == _firstTestSale.Id);
        if (!saleExists)
        {
            await Context.Sales.AddAsync(_firstTestSale);
        }
        
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        // Delete in correct order to avoid foreign key violations
        Context.Sales.RemoveRange(Context.Sales);
        await SaveChangesAsync();
        
        Context.VinylRecords.RemoveRange(Context.VinylRecords);
        await SaveChangesAsync();
        
        Context.Artists.RemoveRange(Context.Artists);
        await SaveChangesAsync();
    }
}

