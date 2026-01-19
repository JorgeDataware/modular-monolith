# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview
This is a **Modular Monolith** ASP.NET Core application built with .NET 10.0. The architecture follows a modular approach where each business domain is organized as a self-contained module with its own domain, application, and infrastructure layers.

## Architecture

### Modular Monolith Pattern
The codebase is organized as a modular monolith where:
- Each module is a self-contained vertical slice with its own domain logic, data access, and API endpoints
- Modules are **internal by default** to enforce encapsulation (Domain entities, DbContext, services are all `internal`)
- Modules expose functionality through extension methods (e.g., `MovieService()`, `UseMoviesModuleMigrations()`)
- The host API project (`CP.Host.Api`) orchestrates modules but cannot directly access their internals

### Current Structure
```
app/backend/
├── CP.Portal.sln                    # Solution file
├── api/src/CP.Portal.Api/           # Host API project
│   ├── CP.Host.Api.csproj
│   ├── Program.cs                   # Application entry point
│   └── appsettings.json
└── Movies.Module/src/CP.Portal.Movies.Module/  # Movies domain module
    ├── Domain/                      # Domain entities (internal)
    ├── Application/                 # Use cases and endpoints (internal)
    │   └── GetListMovies/          # Feature folders per use case
    ├── Infrastructure/              # Data access (internal)
    │   ├── MovieDbContext.cs       # EF Core context
    │   ├── Configurations/         # Entity configurations
    │   └── Migrations/             # EF Core migrations
    └── MovieServiceExtensions.cs   # Public API of the module
```

### Key Architectural Principles
1. **Vertical Slice Architecture**: Each feature/use case is organized in its own folder within `Application/` (e.g., `GetListMovies/`)
2. **Internal by Default**: All domain entities, services, and DbContext are marked `internal` to prevent direct access from outside the module
3. **Schema Isolation**: Each module uses its own database schema (Movies module uses `"movies"` schema)
4. **Extension Methods for Integration**: Modules expose public extension methods for DI registration and middleware configuration
5. **FastEndpoints**: Uses FastEndpoints library instead of traditional ASP.NET Core controllers for endpoint definition

## Development Commands

### Building the Solution
```powershell
# Build from solution directory
dotnet build app\backend\CP.Portal.sln

# Build from root
dotnet build app\backend\CP.Portal.sln --configuration Release
```

### Running the Application
```powershell
# Run the API host project
dotnet run --project app\backend\api\src\CP.Portal.Api\CP.Host.Api.csproj

# The API will be available at:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:7025 or http://localhost:5275
```

### Database Migrations

**IMPORTANT**: Migrations are automatically applied on startup via `UseMoviesModuleMigrations()` in Program.cs.

To create new migrations:
```powershell
# Add migration (must specify context and project paths)
dotnet ef migrations add <MigrationName> `
  --project app\backend\Movies.Module\src\CP.Portal.Movies.Module `
  --startup-project app\backend\api\src\CP.Portal.Api `
  --context MovieDbContext

# Example:
dotnet ef migrations add AddMovieRating `
  --project app\backend\Movies.Module\src\CP.Portal.Movies.Module `
  --startup-project app\backend\api\src\CP.Portal.Api `
  --context MovieDbContext
```

### Configuration
- Connection strings must be added to `appsettings.json` (not tracked in git per .gitignore)
- Movies module expects a connection string named `"MoviesConnectionString"`
- Development settings in `appsettings.Development.json` are gitignored

## Adding a New Module

When creating a new module, follow this pattern:

1. **Create module project structure**:
   ```
   NewModule.Module/src/CP.Portal.NewModule.Module/
   ├── Domain/              # Entities (internal)
   ├── Application/         # Features (internal)
   ├── Infrastructure/      # DbContext, Configurations (internal)
   └── NewModuleServiceExtensions.cs  # Public API
   ```

2. **Make module types internal**: All domain entities, services, DbContext must be `internal`

3. **Create service extensions** (public API of module):
   ```csharp
   public static class NewModuleServiceExtensions
   {
       public static IServiceCollection NewModuleService(
           this IServiceCollection services, 
           ConfigurationManager config)
       {
           // Register module services
           // Register DbContext with connection string
           return services;
       }

       public static IApplicationBuilder UseNewModuleMigrations(
           this IApplicationBuilder app)
       {
           // Apply migrations on startup
           return app;
       }
   }
   ```

4. **Configure DbContext with schema isolation**:
   ```csharp
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       modelBuilder.HasDefaultSchema("newmodule");
       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
   }
   ```

5. **Register in host Program.cs**:
   ```csharp
   builder.Services.NewModuleService(builder.Configuration);
   // ...
   app.UseNewModuleMigrations();
   ```

## FastEndpoints Pattern

Endpoints are defined using FastEndpoints library instead of controllers:

```csharp
internal class GetListMoviesEndpoint(GetListMoviesService movieService) 
    : EndpointWithoutRequest<IEnumerable<MovieDto>>
{
    public override void Configure()
    {
        Get("/api/movies/GetMovies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct=default)
    {
        var movies = await _movieService.GetMovies();
        await Send.OkAsync(movies.ToList());
    }
}
```

FastEndpoints are automatically discovered and registered via `builder.Services.AddFastEndpoints()` and `app.UseFastEndpoints()`.

## Domain Entity Pattern

Entities use:
- **Private setters** to enforce immutability
- **Guid.CreateVersion7()** for ID generation (UUIDv7)
- **ValueGeneratedNever()** in EF configuration since IDs are generated in code
- **Navigation properties** for relationships
- **NotMapped projections** for derived properties (e.g., flattening many-to-many relationships)

Example:
```csharp
internal class Movie
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Title { get; private set; }
    
    // Relations
    public ICollection<MovieGenre> MovieGenres { get; private set; } = [];
    
    // Projection
    [NotMapped]
    public IEnumerable<Genre> Genres => MovieGenres.Select(mg => mg.Genre!);
}
```

## Testing

No test projects currently exist in the solution. When adding tests, follow the module structure and consider using:
- xUnit or NUnit for unit tests
- Integration tests that respect module boundaries (test via public extension methods only)
