using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.Module.Domain;

internal class User : IdentityUser
{
    public string? FullName { get; set; }

    public ICollection<CartMovie> CartMovie { get; set; } = [];
}

internal class UserConf : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.CartMovie)
                .WithOne(cm => cm.User)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}