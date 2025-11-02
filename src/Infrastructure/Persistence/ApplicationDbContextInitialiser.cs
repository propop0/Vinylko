using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Artists;
using Domain.Genres;
using Domain.VinylRecords;
using Domain.Sales;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _dbContext.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (await _dbContext.Genres.AnyAsync())
            {
                return; 
            }
            
            // Seed Artists
            var artist1 = Artist.New(Guid.NewGuid(), "Pink Floyd", "English rock band formed in London in 1965", "United Kingdom", DateTime.Parse("1965-01-01"), "https://www.pinkfloyd.com");
            var artist2 = Artist.New(Guid.NewGuid(), "Led Zeppelin", "English rock band formed in London in 1968", "United Kingdom", DateTime.Parse("1968-01-01"), "https://www.ledzeppelin.com");
            var artist3 = Artist.New(Guid.NewGuid(), "The Beatles", "English rock band formed in Liverpool in 1960", "United Kingdom", DateTime.Parse("1960-01-01"), "https://www.thebeatles.com");
            var artist4 = Artist.New(Guid.NewGuid(), "Queen", "British rock band formed in London in 1970", "United Kingdom", DateTime.Parse("1970-01-01"), "https://www.queenonline.com");

            await _dbContext.Artists.AddRangeAsync(artist1, artist2, artist3, artist4);
            await _dbContext.SaveChangesAsync();

            // Seed Genres
            var genre1 = Genre.New(Guid.NewGuid(), "Rock", "A broad genre of popular music that originated as 'rock and roll' in the United States");
            var genre2 = Genre.New(Guid.NewGuid(), "Progressive Rock", "A subgenre of rock music that developed in the late 1960s and early 1970s");
            var genre3 = Genre.New(Guid.NewGuid(), "Classic Rock", "A radio format which developed from the album-oriented rock format");
            var genre4 = Genre.New(Guid.NewGuid(), "Psychedelic Rock", "A subgenre of rock music that emerged in the mid-1960s");

            await _dbContext.Genres.AddRangeAsync(genre1, genre2, genre3, genre4);
            await _dbContext.SaveChangesAsync();

            // Seed Vinyl Records
            var vinyl1 = VinylRecord.New(Guid.NewGuid(), "The Dark Side of the Moon", genre2.Id.ToString(), 1973, artist1.Id, Guid.NewGuid(), 29.99m, "The eighth studio album by Pink Floyd");
            var vinyl2 = VinylRecord.New(Guid.NewGuid(), "Led Zeppelin IV", genre1.Id.ToString(), 1971, artist2.Id, Guid.NewGuid(), 34.99m, "The fourth studio album by Led Zeppelin");
            var vinyl3 = VinylRecord.New(Guid.NewGuid(), "Abbey Road", genre3.Id.ToString(), 1969, artist3.Id, Guid.NewGuid(), 39.99m, "The twelfth studio album by The Beatles");
            var vinyl4 = VinylRecord.New(Guid.NewGuid(), "Bohemian Rhapsody", genre1.Id.ToString(), 1975, artist4.Id, Guid.NewGuid(), 24.99m, "Single from A Night at the Opera album");
            var vinyl5 = VinylRecord.New(Guid.NewGuid(), "Wish You Were Here", genre2.Id.ToString(), 1975, artist1.Id, Guid.NewGuid(), 27.99m, "The ninth studio album by Pink Floyd");

            await _dbContext.VinylRecords.AddRangeAsync(vinyl1, vinyl2, vinyl3, vinyl4, vinyl5);
            await _dbContext.SaveChangesAsync();

            // Seed Sales
            var sale1 = Sale.New(Guid.NewGuid(), $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}-001", vinyl1.Id, 29.99m, DateTime.UtcNow.AddDays(-5), "John Doe", "john.doe@email.com");
            sale1.Complete("Customer was very satisfied with the purchase");
            
            var sale2 = Sale.New(Guid.NewGuid(), $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}-002", vinyl3.Id, 39.99m, DateTime.UtcNow.AddDays(-2), "Jane Smith", "jane.smith@email.com");
            sale2.Complete("Excellent condition vinyl");

            await _dbContext.Sales.AddRangeAsync(sale1, sale2);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
