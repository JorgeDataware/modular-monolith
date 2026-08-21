using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.Module.Domain;

internal class CartMovie
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    // Usuario
    public User User { get; set; } = null!;
    public string UserId { get; set; } = null!;

    // Movie
    public Guid MovieId { get; set; }
}

internal class CartConf : IEntityTypeConfiguration<CartMovie>
{
    public void Configure(EntityTypeBuilder<CartMovie> builder)
    {
        builder.ToTable("CartMovie");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}