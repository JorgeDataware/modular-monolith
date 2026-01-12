using CP.Portal.Movies.Module;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Inyectar movie service
builder.Services.MovieService();

// Inyectar FastEndpoints
builder.Services.AddFastEndpoints();

builder.Services.AddOpenApi();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapMovieEndpoints();

app.UseFastEndpoints();

app.Run();