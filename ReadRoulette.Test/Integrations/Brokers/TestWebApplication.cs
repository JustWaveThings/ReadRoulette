using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReadRoulette.Persistence;

namespace ReadRoulette.Test;

public class TestWebApplicationFactory<P> : WebApplicationFactory<P> where P : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(svc => {
            var ctx = svc.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (ctx != null)
            {
                svc.Remove(ctx);

                var dbOpts = svc
                    .Where(r => r.ServiceType == typeof(DbContextOptions)
                        || (r.ServiceType.IsGenericType 
                            && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
                    .ToArray();

                foreach (var opt in dbOpts)
                    svc.Remove(opt);
            }

            _ = svc.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("test"));

            var provider = svc.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var scopedSvc = scope.ServiceProvider;
            var db = scopedSvc.GetRequiredService<AppDbContext>();

            SeedUsers(db);
            SeedBooks(db);
            SeedUserBooksToRead(db);
            SeedBookClubs(db);
            SeedBookClubMembers(db);
        });
    }

    private static async void SeedBookClubs(AppDbContext db)
    {
        var user = await db.Users.FirstAsync();
        db.BookClubs.AddRange(
            new BookClub
            {
                Id = string.Join("", Guid.NewGuid().ToString().Split('-')),
                UserId = user.Id,
                Title = "BookClub1"
            }
        );
        await db.SaveChangesAsync();
    }

    private static async void SeedBookClubMembers(AppDbContext db)
    {
        var users = await db.Users.ToListAsync();
        var bookClub = await db.BookClubs.FirstAsync();
        db.BookClubMembers.AddRange(
            new BookClubMember
            {
                BookClubId = bookClub.Id,
                UserId = users.Skip(1).First().Id
            },
            new BookClubMember
            {
                BookClubId = bookClub.Id,
                UserId = users.Skip(2).First().Id
            },
            new BookClubMember
            {
                BookClubId = bookClub.Id,
                UserId = users.Last().Id
            }
        );
        await db.SaveChangesAsync();
    }

    private static async void SeedUserBooksToRead(AppDbContext db)
    {
        var users = await db.Users.ToListAsync();
        db.UserBookToReads.AddRange(
            new UserBookToRead
            {
                ReaderId = users.First().Id,
                BookId = 1
            },
            new UserBookToRead
            {
                ReaderId = users.First().Id,
                BookId = 2
            },
            new UserBookToRead
            {
                ReaderId = users.Last().Id,
                BookId = 3
            }
        );
        await db.SaveChangesAsync();
    }

    private static async void SeedBooks(AppDbContext db)
    {
        db.Books.AddRange(
            new Book
            {
                Title = "Book1",
                Author = "Author1"
            },
            new Book
            {
                Title = "Book2",
                Author = "Author2"
            },
            new Book
            {
                Title = "Book3",
                Author = "Author3"
            },
            new Book
            {
                Title = "Book4",
                Author = "Author4"
            }
        );
        await db.SaveChangesAsync();
    }

    private static async void SeedUsers(AppDbContext db)
    {
        var hasher = new PasswordHasher<IdentityUser>();

        var user = new IdentityUser
        {
            Email = "test@gmail.com"
        };
        user.PasswordHash = hasher.HashPassword(user, "ASecretPw1!");

        var user2 = new IdentityUser
        {
            Email = "test2@gmail.com"
        };
        user2.PasswordHash = hasher.HashPassword(user2, "ASecretPw1!");

        var user3 = new IdentityUser
        {
            Email = "test3@gmail.com"
        };
        user3.PasswordHash = hasher.HashPassword(user3, "ASecretPw1!");

        db.Users.AddRange(user, user2, user3);
        await db.SaveChangesAsync();
    }
}
