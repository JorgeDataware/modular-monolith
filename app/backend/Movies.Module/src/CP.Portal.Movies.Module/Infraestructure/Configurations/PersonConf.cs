using CP.Portal.Movies.Module.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CP.Portal.Movies.Module.Infraestructure.Configurations;

internal class PersonConf : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        // El primer parámetro es el nombre de la tabla y el segundo es el esquema
        builder.ToTable("persons", "movies");
        // Se define la clave primaria Person.Id
        builder.HasKey(p => p.Id);
        // Definir que la base de datos nunca genere los valores para la columna Id
        builder.Property(p => p.Id).ValueGeneratedNever();

        // Definir llave foránea con Cast (Uno a Muchos)
        builder.HasMany(p => p.Casts)
            .WithOne(c => c.Person)
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Definir llave foránea con Crew (Uno a Muchos)
        builder.HasMany(p => p.Crewers)
            .WithOne(c => c.Person)
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
