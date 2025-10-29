using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

Application.ConfigureServices.AddApplicationServices(builder.Services);
Infrastructure.ConfigureInfrastructureServices.AddInfrastructureServices(builder.Services, builder.Configuration);
Api.Modules.SetupModule.AddApiModule(builder.Services);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Api.Filters.ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

app.MapControllers();

app.Run();

// Make Program accessible to tests
[ExcludeFromCodeCoverage]
public partial class Program { }
