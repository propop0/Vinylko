using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureServices();
    }

    private static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ISaleNumberGenerator, SaleNumberGenerator>();
        services.AddScoped<IVinylRecordStatusService, VinylRecordStatusService>();
        services.AddScoped<ISaleStatusService, SaleStatusService>();
        services.AddScoped<IValidationService, ValidationService>();
    }
}