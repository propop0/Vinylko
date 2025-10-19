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
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<EquipmentRepository>();
        services.AddScoped<IEquipmentRepository>(provider => provider.GetRequiredService<EquipmentRepository>());
        services.AddScoped<IEquipmentQueries>(provider => provider.GetRequiredService<EquipmentRepository>());

        services.AddScoped<MaintenanceScheduleRepository>();
        services.AddScoped<IMaintenanceScheduleRepository>(provider => provider.GetRequiredService<MaintenanceScheduleRepository>());
        services.AddScoped<IMaintenanceScheduleQueries>(provider => provider.GetRequiredService<MaintenanceScheduleRepository>());

        services.AddScoped<WorkOrderRepository>();
        services.AddScoped<IWorkOrderRepository>(provider => provider.GetRequiredService<WorkOrderRepository>());
        services.AddScoped<IWorkOrderQueries>(provider => provider.GetRequiredService<WorkOrderRepository>());
    }
}
