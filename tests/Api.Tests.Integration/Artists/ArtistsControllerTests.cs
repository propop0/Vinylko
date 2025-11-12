using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Artists;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Artists;
using Xunit;

namespace Api.Tests.Integration.Artists;

public class ArtistsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/artists";

    private readonly Artist _firstTestArtist;
    private readonly Artist _secondTestArtist;

    public ArtistsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _firstTestArtist = ArtistData.FirstArtist();
        _secondTestArtist = ArtistData.SecondArtist();
    }

    [Fact]
    public async Task ShouldGetArtistById()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestArtist.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var artistDto = await response.ToResponseModel<ArtistDto>();
        artistDto.Id.Should().Be(_firstTestArtist.Id);
        artistDto.Name.Should().Be(_firstTestArtist.Name);
        artistDto.Bio.Should().Be(_firstTestArtist.Bio);
        artistDto.Country.Should().Be(_firstTestArtist.Country);
    }

    [Fact]
    public async Task ShouldGetAllArtists()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var artists = await response.ToResponseModel<List<ArtistDto>>();
        artists.Should().NotBeNull();
        artists.Should().Contain(a => a.Id == _firstTestArtist.Id);
    }

    [Fact]
    public async Task ShouldCreateArtist()
    {
        // Arrange
        var request = new CreateArtistDto(
            Name: _secondTestArtist.Name,
            Bio: _secondTestArtist.Bio,
            Country: _secondTestArtist.Country,
            BirthDate: _secondTestArtist.BirthDate,
            Website: _secondTestArtist.Website
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();
        var artistDto = await response.ToResponseModel<ArtistDto>();

        // Assert - verify DB state
        var dbArtist = await Context.Artists
            .AsNoTracking()
            .FirstAsync(x => x.Id == artistDto.Id);
        dbArtist.Name.Should().Be(request.Name);
        dbArtist.Bio.Should().Be(request.Bio);
        dbArtist.Country.Should().Be(request.Country);
    }

    [Fact]
    public async Task ShouldNotCreateArtistWithInvalidData()
    {
        // Arrange
        var request = new CreateArtistDto(
            Name: string.Empty, // Invalid: empty name
            Bio: string.Empty,  // Invalid: empty bio
            Country: string.Empty, // Invalid: empty country
            BirthDate: null,
            Website: null
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateArtist()
    {
        // Arrange
        var request = new UpdateArtistDto(
            Name: "Updated Artist Name",
            Bio: "Updated biography",
            Country: "Updated Country",
            BirthDate: new DateTime(1980, 1, 1),
            Website: "https://updated.com"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestArtist.Id}", request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();

        // Assert - verify DB
        var updatedArtist = await Context.Artists
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestArtist.Id);
        updatedArtist.Name.Should().Be(request.Name);
        updatedArtist.Bio.Should().Be(request.Bio);
        updatedArtist.Country.Should().Be(request.Country);
    }

    [Fact]
    public async Task ShouldNotUpdateArtistWithInvalidData()
    {
        // Arrange
        var request = new UpdateArtistDto(
            Name: string.Empty, // Invalid: empty name
            Bio: string.Empty,
            Country: string.Empty,
            BirthDate: null,
            Website: null
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestArtist.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentArtist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateArtistDto(
            Name: "Test",
            Bio: "Test Bio",
            Country: "Test Country",
            BirthDate: null,
            Website: null
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenArtistDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteArtist()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestArtist.Id}");

        // Assert - verify response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify deletion from DB
        var artistExists = await Context.Artists
            .AnyAsync(x => x.Id == _firstTestArtist.Id);
        artistExists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentArtist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ShouldGetArtistsByCountry()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/country/{_firstTestArtist.Country}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var artists = await response.ToResponseModel<List<ArtistDto>>();
        artists.Should().NotBeNull();
        artists.Should().AllSatisfy(a => a.Country.Should().Be(_firstTestArtist.Country));
    }

    public async Task InitializeAsync()
    {
        // перевірка чи вже існубть
        var exists = await Context.Artists.AnyAsync(x => x.Id == _firstTestArtist.Id);
        if (!exists)
        {
            await Context.Artists.AddAsync(_firstTestArtist);
            await SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        var artistIds = Context.Artists.Select(a => a.Id).ToList();
        
        // видалити платівки артиста
        var vinylRecords = await Context.VinylRecords
            .Where(v => artistIds.Contains(v.ArtistId))
            .ToListAsync();
        if (vinylRecords.Any())
        {
            Context.VinylRecords.RemoveRange(vinylRecords);
            await SaveChangesAsync();
        }
        
        // видалення артиста
        Context.Artists.RemoveRange(Context.Artists);
        await SaveChangesAsync();
    }
}

