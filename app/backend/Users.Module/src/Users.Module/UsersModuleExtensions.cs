using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Users.Module.Application.Services.Auth;
using Users.Module.Application.Services.CartMovieService;
using Users.Module.Domain;
using Users.Module.Domain.Repositories.CartMovieRepository;
using Users.Module.Infrastructure;
using Users.Module.Utilities.Configuration;

namespace Users.Module;

public static class UsersModuleExtensions
{
    public static IServiceCollection UsersModuleServices(this IServiceCollection services, ConfigurationManager config)
    {
        // Registro DI de servicios
        services.AddScoped<ICartMovieService, CartMovieService>();
        services.AddScoped<IAuthService, AuthService>();

        // Registro DI de repositories
        services.AddScoped<ICartMovieRepository, CartMovieRepository>();

        // Inyección de MovieDbContext en el contenedor de servicios
        string? connectionString = config.GetConnectionString("MoviesConnectionString");
        services.AddDbContext<UsersDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        services.AddOptions<JWTConfigs>()
            .Bind(config.GetSection(JWTConfigs.SectionName))
            .Validate(c => !string.IsNullOrWhiteSpace(c.Secret), "Jwt:Secret no está configurado.")
            .Validate(c => c.ExpirationMinutes > 0, "Jwt:ExpirationMinutes debe ser mayor a 0.")
            .ValidateOnStart();

        // Identity: registra UserManager<User> sobre UsersDbContext.
        // AddIdentityCore (y no AddIdentity) porque el módulo no usa cookies de login.
        services.AddIdentityCore<User>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<UsersDbContext>();

        // Inyección de FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        // REGISTRO DE AUTOMAPPER
        services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });

        // Fábrica de conexiones para Dapper
        services.AddScoped<IUsersConnectionFactory, UsersConnectionFactory>();

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

    /// <summary>
    /// Siembra los datos iniciales del módulo. Debe ejecutarse después de
    /// UseUsersModuleMigrations, porque las tablas ya deben existir.
    /// </summary>
    public static async Task<IApplicationBuilder> UseUsersModuleSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var provider = scope.ServiceProvider;

        await UsersDataSeeder.SeedAsync(
            provider.GetRequiredService<UserManager<User>>(),
            provider.GetRequiredService<IConfiguration>(),
            provider.GetRequiredService<ILoggerFactory>().CreateLogger("Users.Module.Seed"));

        return app;
    }
}
