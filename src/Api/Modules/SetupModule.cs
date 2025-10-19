using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Modules;

public static class SetupModule
{
    public static void AddApiModule(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(SetupModule).Assembly);
    }
}