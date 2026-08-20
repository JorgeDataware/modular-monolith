using CP.Portal.Movies.Module;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Inyectar movie service
builder.Services.MovieService(builder.Configuration);

// Inyectar FastEndpoints
builder.Services.AddFastEndpoints();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMoviesModuleMigrations();

//app.UseHttpsRedirection();

// Enable Minimal API endpoints for Movies module
//app.MapMovieEndpoints();

app.UseFastEndpoints(config =>
{
    // operationId = solo el nombre de la clase, sin el namespace completo
    config.Endpoints.ShortNames = true;

    // Agrupa cada endpoint en OpenAPI por el segmento de ruta posterior a "api"
    config.Endpoints.Configurator = ep =>
    {
        var tag = TagFromRoute(ep.Routes?.FirstOrDefault());

        if (tag is not null)
            ep.Options(b => b.WithTags(tag));
    };
});

if (app.Environment.IsDevelopment())
{
    // Documento OpenAPI en /openapi/v1.json
    app.MapOpenApi();

    // UI de Scalar en /scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("CP.Portal API")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.Run();

// "/api/movies/GetMovies" -> "Movies" | "/api/Persons/{Id}" -> "Persons"
static string? TagFromRoute(string? route)
{
    if (string.IsNullOrWhiteSpace(route))
        return null;

    var segment = route
        .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(s => !s.StartsWith('{'))
        .FirstOrDefault(s => !s.Equals("api", StringComparison.OrdinalIgnoreCase));

    return segment is null
        ? null
        : char.ToUpperInvariant(segment[0]) + segment[1..];
}
