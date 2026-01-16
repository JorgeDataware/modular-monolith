using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CP.Portal.Movies.Module.Infraestructure.Configurations;

internal class GenreConf : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // El primer parámetro es el nombre de la tabla y el segundo es el esquema
        builder.ToTable("genres", "movies");
        // Se define la clave primaria Genre.Id
        builder.HasKey(g => g.Id);
        // Definir que la base de datos nunca genere los valores para la columna Id
        builder.Property(g => g.Id).ValueGeneratedNever();
        // Definir llave foránea con MovieGenre (Uno a Muchos)
        builder.HasMany(g => g.MovieGenres)
            .WithOne(mg => mg.Genre)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
