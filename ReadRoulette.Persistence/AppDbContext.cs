using Microsoft.EntityFrameworkCore;

namespace ReadRoulette.Persistence;

public class AppDbContext(DbContextOptions options) 
    : DbContext(options)
{
    public DbSet<Book> Books { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Book>()
            .Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(80);
    }
}
