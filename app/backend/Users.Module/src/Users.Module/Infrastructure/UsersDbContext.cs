using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.Module.Domain;

namespace Users.Module.Infrastructure;

internal class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {

    }

    internal DbSet<User> User { get; set; }
    internal DbSet<CartMovie> CartMovie { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        base.OnModelCreating(modelBuilder);
    }
}
