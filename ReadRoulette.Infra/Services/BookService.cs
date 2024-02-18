using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadRoulette.Domain;
using ReadRoulette.Persistence;

namespace ReadRoulette.Infra;

public class BookService(
  AppDbContext _ctx,
  IHttpContextAccessor _httpCtx,
  UserManager<IdentityUser> _userManager) : IBookService
{
    private string GetUserId()
    {
        var currentUser = _httpCtx.HttpContext?.User ?? throw new Exception("A user was not present on the HttpContext.");
        var userId = _userManager.GetUserId(currentUser) ?? throw new Exception("A user was not present.");

        return userId;
    }

	private async Task<UserBookToRead?> GetLatestUserBookByUserId(string userId) =>
		await _ctx.UserBookToReads
			.Where(b => b.Order != 0 && b.ReaderId == userId)
			.OrderBy(b => b.Order)
			.FirstOrDefaultAsync();

    public async Task<List<UserBookToRead>> AddBookToReadListAsync(Book newBook)
    {
        string userId = GetUserId();

        if (await _ctx.Books.FindAsync(newBook.Id) is Book found)
        {
            _ctx.UserBookToReads.Add(new()
            {
              ReaderId = userId,
              BookId = found.Id
            });
        }
        else
        {
            var addedBook = await CreateBookAsync(newBook) 
              ?? throw new Exception("Failure adding book.");
            _ctx.UserBookToReads.Add(new()
            {
              ReaderId = userId,
              BookId = addedBook.Id
            });
        }
        await _ctx.SaveChangesAsync();

        return await GetUserBookToReadByUserIdAsync(userId);
    }

    public async Task<Book?> CreateBookAsync(Book newBook)
    {
        var result = _ctx.Books.Add(newBook);
        await _ctx.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<bool> DeleteBookByIdAsync(int id)
    {
        if (await _ctx.Books.FindAsync(id) is not Book found)
          return false;

        _ctx.Books.Remove(found);
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        if (await _ctx.Books.FindAsync(id) is not Book found)
          return null;

        return found;
    }

    public async Task<List<Book>> GetBooksListAsync() =>
        await _ctx.Books.ToListAsync();

    public async Task<List<UserBookToRead>> GetUserBookToReadByUserIdAsync(string userId) =>
        await _ctx.UserBookToReads.Where(b => b.ReaderId == userId).ToListAsync();

    public async Task<UserBookToRead?> GetNextBookToReadAsync()
    {
        string userId = GetUserId();

		var firstBook = await GetLatestUserBookByUserId(userId);
		if (firstBook is null)
			return await _ctx.UserBookToReads.FirstOrDefaultAsync();

		firstBook.Order = 0;
		await _ctx.SaveChangesAsync();

		return await GetLatestUserBookByUserId(userId);
    }

    public async Task<List<UserBookToRead>> RandomizeBookListAsync()
    {
        string userId = GetUserId();
		var random = new Random();

		var bookList = await GetUserBookToReadByUserIdAsync(userId);
		foreach (var book in bookList)
			book.Order = random.Next(1, bookList.Count);

		await _ctx.SaveChangesAsync();
		return bookList;
    }

    public async Task<List<UserBookToRead>> RemoveBookFromReadListAsync(int id)
    {
        string userId = GetUserId();

        if (await _ctx.UserBookToReads.FindAsync(id) is not UserBookToRead found)
          return await GetUserBookToReadByUserIdAsync(userId);

        if (found.ReaderId != userId)
          return await GetUserBookToReadByUserIdAsync(userId);

        _ctx.Remove(found);
        await _ctx.SaveChangesAsync();

        return await GetUserBookToReadByUserIdAsync(userId);
    }
}
