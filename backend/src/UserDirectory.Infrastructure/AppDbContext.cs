using Microsoft.EntityFrameworkCore;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.City).IsRequired();
            b.Property(x => x.State).IsRequired();
            b.Property(x => x.Pincode).HasMaxLength(10).IsRequired();
        });
    }
}
