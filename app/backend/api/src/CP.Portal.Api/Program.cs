using CP.Portal.Movies.Module;
using FastEndpoints;

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

app.UseFastEndpoints();

app.Run();