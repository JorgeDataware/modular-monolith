using Microsoft.Extensions.DependencyInjection;

namespace Core.Contracts;

public static class ContractsModuleExtensions
{
    public static IServiceCollection ContractsModuleServices(this IServiceCollection services)
    {
        return services;
    }
}
