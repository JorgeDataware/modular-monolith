using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CP.Portal.Movies.Module.Infraestructure.Configurations;

internal class MovieConf : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        // El primer parámetro es el nombre de la tabla y el segundo es el esquema
        builder.ToTable("movies", "movies");
        // Se define la clave primaria Movie.Id
        builder.HasKey(m => m.Id);
        // Definir que la base de datos nunca genere los valores para la columna Id
        builder.Property(m => m.Id).ValueGeneratedNever();

        // Definir llave foránea con MovieGenre (Uno a Muchos)
        builder.HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Definir llave foránea con Cast (Uno a Muchos)
        builder.HasMany(m => m.Casts)
            .WithOne(c => c.Movie)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Definir llave foránea con Crew (Uno a Muchos)
        builder.HasMany(m => m.Crewers)
            .WithOne(c => c.Movie)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
