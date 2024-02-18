using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReadRoulette.Persistence;

public class AppDbContext(DbContextOptions options) 
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Book> Books { get; set;}
    public DbSet<UserBookToRead> UserBookToReads { get; set; }
    public DbSet<BookClub> BookClubs { get; set; }
    public DbSet<BookClubMember> BookClubMembers { get; set; }

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

        #region Identity

        modelBuilder.Entity<IdentityUser>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<IdentityUser>()
            .Property(u => u.Email)
            .IsRequired();
        modelBuilder.Entity<IdentityUser>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasNoKey();

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasNoKey();

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasNoKey();

        #endregion
    }
}
