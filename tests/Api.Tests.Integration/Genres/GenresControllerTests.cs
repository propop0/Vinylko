using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Genres;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Genres;
using Xunit;

namespace Api.Tests.Integration.Genres;

public class GenresControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/genres";

    private readonly Genre _firstTestGenre;
    private readonly Genre _secondTestGenre;

    public GenresControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _firstTestGenre = GenreData.FirstGenre();
        _secondTestGenre = GenreData.SecondGenre();
    }

    [Fact]
    public async Task ShouldGetGenreById()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestGenre.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var genreDto = await response.ToResponseModel<GenreDto>();
        genreDto.Id.Should().Be(_firstTestGenre.Id);
        genreDto.Name.Should().Be(_firstTestGenre.Name);
        genreDto.Description.Should().Be(_firstTestGenre.Description);
    }

    [Fact]
    public async Task ShouldGetAllGenres()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var genres = await response.ToResponseModel<List<GenreDto>>();
        genres.Should().NotBeNull();
        genres.Should().Contain(g => g.Id == _firstTestGenre.Id);
    }

    [Fact]
    public async Task ShouldCreateGenre()
    {
        // Arrange 
        var thirdTestGenre = GenreData.ThirdGenre(); 
        var request = new CreateGenreDto(
            Name: thirdTestGenre.Name, 
            Description: thirdTestGenre.Description
        );
        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        // Assert 
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotCreateGenreWithInvalidData()
    {
        // Arrange
        var request = new CreateGenreDto(
            Name: string.Empty, // помилка пусте ім'я
            Description: string.Empty // помилка пустий опив
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateGenre()
    {
        // Arrange 
        var request = new CreateGenreDto(
            Name: _firstTestGenre.Name, 
            Description: "Different description"
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert 
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Conflict, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateGenre()
    {
        // Arrange
        var request = new UpdateGenreDto(
            Name: "Updated Genre Name",
            Description: "Updated description"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestGenre.Id}", request);

        // Assert - перевірка відповіді шттп
        response.IsSuccessStatusCode.Should().BeTrue();

        // Assert - verify DB
        var updatedGenre = await Context.Genres
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestGenre.Id);
        updatedGenre.Name.Should().Be(request.Name);
        updatedGenre.Description.Should().Be(request.Description);
    }

    [Fact]
    public async Task ShouldNotUpdateGenreWithInvalidData()
    {
        // Arrange
        var request = new UpdateGenreDto(
            Name: string.Empty, // помилка пусте ім'я
            Description: string.Empty
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestGenre.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentGenre()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateGenreDto(
            Name: "Test",
            Description: "Test Description"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenGenreDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteGenre()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestGenre.Id}");

        // Assert - перевірка відповіді
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - перевірка видалення
        var genreExists = await Context.Genres
            .AnyAsync(x => x.Id == _firstTestGenre.Id);
        genreExists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentGenre()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldGetGenreByName()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/name/{_firstTestGenre.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var genreDto = await response.ToResponseModel<GenreDto>();
        genreDto.Name.Should().Be(_firstTestGenre.Name);
    }

    public async Task InitializeAsync()
    {
        var exists = await Context.Genres.AnyAsync(x => x.Id == _firstTestGenre.Id);
        if (!exists)
        {
            await Context.Genres.AddAsync(_firstTestGenre);
            await SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        Context.Sales.RemoveRange(Context.Sales);
        Context.VinylRecords.RemoveRange(Context.VinylRecords);
        Context.Artists.RemoveRange(Context.Artists);
        Context.Genres.RemoveRange(Context.Genres);
        await SaveChangesAsync();
    }
}
