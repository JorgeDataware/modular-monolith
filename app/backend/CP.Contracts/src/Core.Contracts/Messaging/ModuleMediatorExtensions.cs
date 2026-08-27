using System.Reflection;
using Core.Contracts.Messaging.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Contracts.Messaging;

public static class ModuleMediatorExtensions
{
    /// <summary>
    /// Ruta en configuración de la licencia de MediatR (Lucky Penny Software).
    /// Alternativas si no se quiere en appsettings: variable de entorno
    /// MEDIATR_LICENSE_KEY o LUCKYPENNY_LICENSE_KEY, que MediatR lee por su cuenta.
    /// </summary>
    private const string LicenseKeyPath = "MediatR:LicenseKey";

    /// <summary>
    /// Registra el mediador para UN módulo: descubre sus handlers y engancha
    /// los behaviors compartidos.
    ///
    /// Cada módulo llama a esto desde su propio *ModuleExtensions, de modo que el host
    /// (Program.cs) no necesita conocer ni los handlers ni los ensamblados de nadie.
    /// Es seguro invocarlo varias veces: MediatR acumula handlers por ensamblado y los
    /// behaviors se registran con TryAddEnumerable para que no se dupliquen.
    /// </summary>
    public static IServiceCollection AddModuleMediator(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly moduleAssembly)
    {
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = configuration[LicenseKeyPath];
            cfg.RegisterServicesFromAssembly(moduleAssembly);
        });

        // Los behaviors se registran como genéricos abiertos: el contenedor los cierra
        // para cada par (request, response). El orden de registro es el orden de ejecución.
        services.TryAddEnumerable(ServiceDescriptor.Transient(
            typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)));

        return services;
    }
}
