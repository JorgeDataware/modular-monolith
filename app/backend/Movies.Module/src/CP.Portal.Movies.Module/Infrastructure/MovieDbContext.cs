using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CP.Portal.Movies.Module.Infrastructure;

internal class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {

    }

    internal DbSet<Movie> movies { get; set; }
    internal DbSet<Genre> genres { get; set; }
    internal DbSet<MovieGenre> movieGenres { get; set; }
    internal DbSet<Person> persons { get; set; }
    internal DbSet<Cast> casters { get; set; }
    internal DbSet<Crew> crewers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("movies");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
