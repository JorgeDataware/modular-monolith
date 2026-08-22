using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Users.Module.Domain;

namespace Users.Module.Infrastructure;

internal static class UsersDataSeeder
{
    /// <summary>
    /// Crea el usuario inicial si no existe. Es idempotente: se puede ejecutar
    /// en cada arranque sin duplicar datos.
    /// Las credenciales salen de la configuración (sección "Users:Seed"), nunca
    /// del código, para no commitear una contraseña al repositorio.
    /// </summary>
    public static async Task SeedAsync(
        UserManager<User> userManager,
        IConfiguration config,
        ILogger logger)
    {
        var email = config["Users:Seed:Email"];
        var password = config["Users:Seed:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogInformation(
                "Seed de usuarios omitido: no hay credenciales en la sección Users:Seed.");
            return;
        }

        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = config["Users:Seed:FullName"] ?? "Administrador"
        };

        // CreateAsync hashea la contraseña, normaliza el email y genera los stamps.
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"No se pudo crear el usuario semilla: {errors}");
        }

        logger.LogInformation("Usuario semilla {Email} creado.", email);
    }
}
