using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CP.Portal.Movies.Module.Infrastructure.Configurations;

internal class MovieGenreConf : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        // El primer parámetro es el nombre de la tabla y el segundo es el esquema
        builder.ToTable("movie_genres", "movies");
        // Se define la clave primaria compuesta MovieGenre.MovieId + MovieGenre.GenreId
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
        // Definir llave foránea con Movie (Muchos a Uno)
        builder.HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        // Definir llave foránea con Genre (Muchos a Uno)
        builder.HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Crear un índice único para acelerar las consultas
        builder.HasIndex(mg => mg.GenreId);
        builder.HasIndex(mg => mg.MovieId);
    }
}
