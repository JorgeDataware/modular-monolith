using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CP.Portal.Movies.Module.Infrastructure.Configurations;

internal class CrewConf : IEntityTypeConfiguration<Crew>
{
    public void Configure(EntityTypeBuilder<Crew> builder)
    {
        // El primer parámetro es el nombre de la tabla y el segundo es el esquema
        builder.ToTable("crewers", "movies");
        // Se define la clave primaria compuesta
        builder.HasKey(c => new { c.MovieId, c.PersonId });
        // Definir que la base de datos nunca genere los valores para la columna Id
        builder.Property(c => c.Id).ValueGeneratedNever();
        // Crear un índice único para acelerar las consultas
        builder.HasIndex(c => c.MovieId);
        builder.HasIndex(c => c.PersonId);

        // Definir llave foránea con Movie (Uno a Muchos)
        builder.HasOne(c => c.Movie)
            .WithMany(m => m.Crewers)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Definir llave foránea con Person (Uno a Muchos)
        builder.HasOne(c => c.Person)
            .WithMany(p => p.Crewers)
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
