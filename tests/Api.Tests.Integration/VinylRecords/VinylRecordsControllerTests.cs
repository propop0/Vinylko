using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Artists;
using Domain.VinylRecords;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Artists;
using Tests.Data.VinylRecords;
using Xunit;

namespace Api.Tests.Integration.VinylRecords;

public class VinylRecordsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/vinyl-records";

    private readonly Artist _testArtist;
    private readonly VinylRecord _firstTestVinylRecord;
    private readonly VinylRecord _secondTestVinylRecord;
    private readonly Guid _testLabelId;

    public VinylRecordsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _testArtist = ArtistData.FirstArtist();
        _testLabelId = Guid.NewGuid();
        _firstTestVinylRecord = VinylRecordData.FirstVinylRecord(_testArtist.Id, _testLabelId);
        _secondTestVinylRecord = VinylRecordData.SecondVinylRecord(_testArtist.Id, _testLabelId);
    }

    [Fact]
    public async Task ShouldGetVinylRecordById()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestVinylRecord.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vinylRecordDto = await response.ToResponseModel<VinylRecordDto>();
        vinylRecordDto.Id.Should().Be(_firstTestVinylRecord.Id);
        vinylRecordDto.Title.Should().Be(_firstTestVinylRecord.Title);
        vinylRecordDto.ArtistId.Should().Be(_firstTestVinylRecord.ArtistId);
        vinylRecordDto.Price.Should().Be(_firstTestVinylRecord.Price);
    }

    [Fact]
    public async Task ShouldGetAllVinylRecords()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vinylRecords = await response.ToResponseModel<List<VinylRecordDto>>();
        vinylRecords.Should().NotBeNull();
        vinylRecords.Should().Contain(vr => vr.Id == _firstTestVinylRecord.Id);
    }

    [Fact]
    public async Task ShouldCreateVinylRecord()
    {
        // Arrange
        var request = new CreateVinylRecordDto(
            Title: _secondTestVinylRecord.Title,
            Genre: _secondTestVinylRecord.Genre,
            ReleaseYear: _secondTestVinylRecord.ReleaseYear,
            ArtistId: _testArtist.Id,
            LabelId: _testLabelId,
            Price: _secondTestVinylRecord.Price,
            Description: _secondTestVinylRecord.Description
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();
        var vinylRecordDto = await response.ToResponseModel<VinylRecordDto>();

        // Assert - verify DB state
        var dbVinylRecord = await Context.VinylRecords
            .AsNoTracking()
            .FirstAsync(x => x.Id == vinylRecordDto.Id);
        dbVinylRecord.Title.Should().Be(request.Title);
        dbVinylRecord.Genre.Should().Be(request.Genre);
        dbVinylRecord.ArtistId.Should().Be(request.ArtistId);
        dbVinylRecord.Price.Should().Be(request.Price);
    }

    [Fact]
    public async Task ShouldNotCreateVinylRecordWithInvalidData()
    {
        // Arrange
        var request = new CreateVinylRecordDto(
            Title: string.Empty, // Invalid: empty title
            Genre: string.Empty, // Invalid: empty genre
            ReleaseYear: 1700, // Invalid: too old
            ArtistId: _testArtist.Id,
            LabelId: _testLabelId,
            Price: -10m, // Invalid: negative price
            Description: null
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateVinylRecord()
    {
        // Arrange - _firstTestVinylRecord already exists in DB (from InitializeAsync)
        var request = new CreateVinylRecordDto(
            Title: _firstTestVinylRecord.Title, // Duplicate: same title
            Genre: "Different Genre",
            ReleaseYear: 2000,
            ArtistId: _testArtist.Id, // Same artist - violates unique constraint on (Title, ArtistId)
            LabelId: _testLabelId,
            Price: 50.00m,
            Description: "Different description"
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Conflict, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateVinylRecord()
    {
        // Arrange
        var request = new UpdateVinylRecordDto(
            Title: "Updated Title",
            Genre: "Updated Genre",
            ReleaseYear: 2020,
            Price: 45.99m,
            Description: "Updated description"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestVinylRecord.Id}", request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();

        // Assert - verify DB
        var updatedVinylRecord = await Context.VinylRecords
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestVinylRecord.Id);
        updatedVinylRecord.Title.Should().Be(request.Title);
        updatedVinylRecord.Genre.Should().Be(request.Genre);
        updatedVinylRecord.Price.Should().Be(request.Price);
    }

    [Fact]
    public async Task ShouldNotUpdateVinylRecordWithInvalidData()
    {
        // Arrange
        var request = new UpdateVinylRecordDto(
            Title: string.Empty, // Invalid: empty title
            Genre: string.Empty,
            ReleaseYear: 1700,
            Price: -10m,
            Description: null
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestVinylRecord.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentVinylRecord()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateVinylRecordDto(
            Title: "Test",
            Genre: "Test Genre",
            ReleaseYear: 2000,
            Price: 25.00m,
            Description: null
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenVinylRecordDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteVinylRecord()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestVinylRecord.Id}");

        // Assert - verify response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify deletion from DB
        var vinylRecordExists = await Context.VinylRecords
            .AnyAsync(x => x.Id == _firstTestVinylRecord.Id);
        vinylRecordExists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentVinylRecord()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldChangeVinylRecordStatus()
    {
        // Arrange
        var request = new ChangeVinylRecordStatusDto(Status: "Reserved");

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestVinylRecord.Id}/status", request);

        // Assert - verify HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify DB
        var updatedVinylRecord = await Context.VinylRecords
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestVinylRecord.Id);
        updatedVinylRecord.Status.Should().Be(VinylRecordStatus.Reserved);
    }

    [Fact]
    public async Task ShouldGetVinylRecordsByArtist()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/artist/{_testArtist.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vinylRecords = await response.ToResponseModel<List<VinylRecordDto>>();
        vinylRecords.Should().NotBeNull();
        vinylRecords.Should().AllSatisfy(vr => vr.ArtistId.Should().Be(_testArtist.Id));
    }

    [Fact]
    public async Task ShouldGetVinylRecordsByStatus()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/status/InStock");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vinylRecords = await response.ToResponseModel<List<VinylRecordDto>>();
        vinylRecords.Should().NotBeNull();
        vinylRecords.Should().AllSatisfy(vr => vr.Status.Should().Be("InStock"));
    }

    [Fact]
    public async Task ShouldSearchVinylRecordsByTitle()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/search?title={_firstTestVinylRecord.Title}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vinylRecords = await response.ToResponseModel<List<VinylRecordDto>>();
        vinylRecords.Should().NotBeNull();
        vinylRecords.Should().Contain(vr => vr.Title.Contains(_firstTestVinylRecord.Title, StringComparison.OrdinalIgnoreCase));
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
        var vinylRecordExists = await Context.VinylRecords.AnyAsync(x => x.Id == _firstTestVinylRecord.Id);
        if (!vinylRecordExists)
        {
            await Context.VinylRecords.AddAsync(_firstTestVinylRecord);
        }
        
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        // Delete in correct order to avoid foreign key violations
        var vinylRecordIds = Context.VinylRecords.Select(v => v.Id).ToList();
        
        // First delete Sales that reference these VinylRecords
        var sales = await Context.Sales
            .Where(s => vinylRecordIds.Contains(s.RecordId))
            .ToListAsync();
        if (sales.Any())
        {
            Context.Sales.RemoveRange(sales);
            await SaveChangesAsync();
        }
        
        // Then delete VinylRecords
        Context.VinylRecords.RemoveRange(Context.VinylRecords);
        await SaveChangesAsync();
        
        // Finally delete Artists
        Context.Artists.RemoveRange(Context.Artists);
        await SaveChangesAsync();
    }
}

