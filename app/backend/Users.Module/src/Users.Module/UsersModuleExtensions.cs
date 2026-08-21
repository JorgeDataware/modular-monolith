using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Module.Infrastructure;

namespace Users.Module;

public static class UsersModuleExtensions
{
    public static IServiceCollection UsersModuleServices(this IServiceCollection services, ConfigurationManager config)
    {
        // Inyección de MovieDbContext en el contenedor de servicios
        string? connectionString = config.GetConnectionString("MoviesConnectionString");
        services.AddDbContext<UsersDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        // Inyección de FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        // REGISTRO DE AUTOMAPPER
        services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });

        return services;
    }

    public static IApplicationBuilder UseUsersModuleMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            // Aquí dentro SÍ podemos ver MovieDbContext porque estamos dentro del módulo
            var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            dbContext.Database.Migrate();
        }
        return app;
    }
}
