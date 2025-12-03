using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Artists;
using Domain.VinylRecords;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Artists;
using Tests.Data.VinylRecordComments;
using Tests.Data.VinylRecords;
using Xunit;

namespace Api.Tests.Integration.VinylRecordComments;

public class VinylRecordCommentsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/vinyl-records";
    private const string CommentsRoute = "/comments";

    private readonly Artist _testArtist;
    private readonly VinylRecord _testVinylRecord;
    private readonly VinylRecordComment _firstTestComment;
    private readonly VinylRecordComment _secondTestComment;

    public VinylRecordCommentsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _testArtist = ArtistData.FirstArtist();
        _testVinylRecord = VinylRecordData.FirstVinylRecord(_testArtist.Id);
        _firstTestComment = VinylRecordCommentData.FirstComment(_testVinylRecord.Id);
        _secondTestComment = VinylRecordCommentData.SecondComment(_testVinylRecord.Id);
    }

    [Fact]
    public async Task ShouldGetCommentsForVinylRecord()
    {
        // Arrange - data preparation in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_testVinylRecord.Id}{CommentsRoute}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var comments = await response.ToResponseModel<List<VinylRecordCommentDto>>();
        comments.Should().NotBeNull();
        comments.Should().Contain(c => c.Id == _firstTestComment.Id);
        comments.Should().Contain(c => c.VinylRecordId == _testVinylRecord.Id);
    }

    [Fact]
    public async Task ShouldCreateComment()
    {
        // Arrange
        var request = new CreateVinylRecordCommentDto
        {
            Content = "This is a new test comment"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseRoute}/{_testVinylRecord.Id}{CommentsRoute}", request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();
        var commentDto = await response.ToResponseModel<VinylRecordCommentDto>();
        commentDto.Content.Should().Be(request.Content);
        commentDto.VinylRecordId.Should().Be(_testVinylRecord.Id);

        // Assert - verify DB state
        var dbComment = await Context.VinylRecordComments
            .AsNoTracking()
            .FirstAsync(x => x.Id == commentDto.Id);
        dbComment.Content.Should().Be(request.Content);
        dbComment.VinylRecordId.Should().Be(_testVinylRecord.Id);
    }

    [Fact]
    public async Task ShouldNotCreateCommentWithInvalidData()
    {
        // Arrange
        var request = new CreateVinylRecordCommentDto
        {
            Content = string.Empty // Invalid: empty content
        };

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseRoute}/{_testVinylRecord.Id}{CommentsRoute}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateCommentForNonExistentVinylRecord()
    {
        // Arrange
        var nonExistentVinylRecordId = Guid.NewGuid();
        var request = new CreateVinylRecordCommentDto
        {
            Content = "This comment should not be created"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseRoute}/{nonExistentVinylRecordId}{CommentsRoute}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldUpdateComment()
    {
        // Arrange
        var request = new UpdateVinylRecordCommentDto
        {
            Content = "Updated comment content"
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/vinyl-record-comments/{_firstTestComment.Id}", request);

        // Assert - verify HTTP response
        response.IsSuccessStatusCode.Should().BeTrue();
        var commentDto = await response.ToResponseModel<VinylRecordCommentDto>();
        commentDto.Content.Should().Be(request.Content);

        // Assert - verify DB
        var updatedComment = await Context.VinylRecordComments
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestComment.Id);
        updatedComment.Content.Should().Be(request.Content);
        updatedComment.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldNotUpdateCommentWithInvalidData()
    {
        // Arrange
        var request = new UpdateVinylRecordCommentDto
        {
            Content = string.Empty // Invalid: empty content
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/vinyl-record-comments/{_firstTestComment.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentComment()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateVinylRecordCommentDto
        {
            Content = "Updated content"
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/vinyl-record-comments/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteComment()
    {
        // Act
        var response = await Client.DeleteAsync($"/api/vinyl-record-comments/{_secondTestComment.Id}");

        // Assert - verify response
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert - verify deletion from DB
        var commentExists = await Context.VinylRecordComments
            .AnyAsync(x => x.Id == _secondTestComment.Id);
        commentExists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentComment()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/api/vinyl-record-comments/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        // Ensure artist exists
        var artistExists = await Context.Artists.AnyAsync(x => x.Id == _testArtist.Id);
        if (!artistExists)
        {
            await Context.Artists.AddAsync(_testArtist);
            await SaveChangesAsync();
        }

        // Ensure vinyl record exists
        var vinylRecordExists = await Context.VinylRecords.AnyAsync(x => x.Id == _testVinylRecord.Id);
        if (!vinylRecordExists)
        {
            await Context.VinylRecords.AddAsync(_testVinylRecord);
            await SaveChangesAsync();
        }

        // Ensure first comment exists
        var firstCommentExists = await Context.VinylRecordComments.AnyAsync(x => x.Id == _firstTestComment.Id);
        if (!firstCommentExists)
        {
            await Context.VinylRecordComments.AddAsync(_firstTestComment);
            await SaveChangesAsync();
        }

        // Ensure second comment exists
        var secondCommentExists = await Context.VinylRecordComments.AnyAsync(x => x.Id == _secondTestComment.Id);
        if (!secondCommentExists)
        {
            await Context.VinylRecordComments.AddAsync(_secondTestComment);
            await SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        // Delete comments
        var comments = await Context.VinylRecordComments
            .Where(c => c.VinylRecordId == _testVinylRecord.Id)
            .ToListAsync();
        if (comments.Any())
        {
            Context.VinylRecordComments.RemoveRange(comments);
            await SaveChangesAsync();
        }

        // Delete vinyl records
        var vinylRecords = await Context.VinylRecords
            .Where(v => v.ArtistId == _testArtist.Id)
            .ToListAsync();
        if (vinylRecords.Any())
        {
            Context.VinylRecords.RemoveRange(vinylRecords);
            await SaveChangesAsync();
        }

        // Delete artist
        var artists = await Context.Artists
            .Where(a => a.Id == _testArtist.Id)
            .ToListAsync();
        if (artists.Any())
        {
            Context.Artists.RemoveRange(artists);
            await SaveChangesAsync();
        }
    }
}

