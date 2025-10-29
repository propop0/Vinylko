using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection"); 
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql( 
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ArtistRepository>();
        services.AddScoped<IArtistRepository>(provider => provider.GetRequiredService<ArtistRepository>());
        services.AddScoped<IArtistQueries>(provider => provider.GetRequiredService<ArtistRepository>());

        services.AddScoped<GenreRepository>();
        services.AddScoped<IGenreRepository>(provider => provider.GetRequiredService<GenreRepository>());
        services.AddScoped<IGenreQueries>(provider => provider.GetRequiredService<GenreRepository>());

        services.AddScoped<VinylRecordRepository>();
        services.AddScoped<IVinylRecordRepository>(provider => provider.GetRequiredService<VinylRecordRepository>());
        services.AddScoped<IVinylRecordQueries>(provider => provider.GetRequiredService<VinylRecordRepository>());

        services.AddScoped<SaleRepository>();
        services.AddScoped<ISaleRepository>(provider => provider.GetRequiredService<SaleRepository>());
        services.AddScoped<ISaleQueries>(provider => provider.GetRequiredService<SaleRepository>());
    }
}
