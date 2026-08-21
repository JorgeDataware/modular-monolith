using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Users.Module;

public static class UsersModuleExtensions
{
    public static IServiceCollection UsersModuleServices(this IServiceCollection services)
    {
        return services;
    }
}
