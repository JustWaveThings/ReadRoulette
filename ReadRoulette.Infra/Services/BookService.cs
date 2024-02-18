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
	/// <summary>
	/// Gets the id of the current <see cref="IdentityUser"/>.
	/// </summary>
	/// <returns>A string id.</returns>
	/// <exception cref="Exception"></exception>
    private string GetUserId()
    {
        var currentUser = _httpCtx.HttpContext?.User ?? throw new Exception("A user was not present on the HttpContext.");
        var userId = _userManager.GetUserId(currentUser) ?? throw new Exception("A user was not present.");

        return userId;
    }

	/// <summary>
	/// Gets the latest <see cref="UserBookToRead"/> entry for a <see cref="IdentityUser"/>.
	/// </summary>
	/// <param name="userId">The id of the <see cref="IdentityUser"/>.</param>
	/// <returns>A <see cref="UserBookToRead"/> entity.</returns>
	private async Task<UserBookToRead?> GetLatestUserBookByUserId(string userId) =>
		await _ctx.UserBookToReads
			.Where(b => b.Order != 0 && b.ReaderId == userId)
			.OrderBy(b => b.Order)
			.FirstOrDefaultAsync();

	/// <summary>
	/// Creates a <see cref="UserBookToRead"/> model relationship between
	/// a <see cref="IdentityUser"/> and <see cref="Book"/>.
	/// </summary>
	/// <param name="newBook">The <see cref="Book"/> to establish for the <see cref="IdentityUser"/>.</param>
	/// <returns>The new <see cref="UserBookToRead"/> entity.</returns>
	/// <exception cref="Exception"></exception>
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

	/// <summary>
	/// Creates a <see cref="Book"/> in the system.
	/// </summary>
	/// <param name="newBook">The <see cref="Book"/> to add.</param>
	/// <returns>The created <see cref="Book"/>.</returns>
    public async Task<Book?> CreateBookAsync(Book newBook)
    {
        var result = _ctx.Books.Add(newBook);
        await _ctx.SaveChangesAsync();

        return result.Entity;
    }

	/// <summary>
	/// Removes a single <see cref="Book"/> from the system.
	/// </summary>
	/// <param name="id">The id of the <see cref="Book"/> to remove.</param>
	/// <returns>A boolean indicating success or failure.</returns>
    public async Task<bool> DeleteBookByIdAsync(int id)
    {
        if (await _ctx.Books.FindAsync(id) is not Book found)
          return false;

        _ctx.Books.Remove(found);
        await _ctx.SaveChangesAsync();
        return true;
    }

	/// <summary>
	/// Gets a single <see cref="Book"/> by its Id.
	/// </summary>
	/// <param name="id">The id of the <see cref="Book"/>.</param>
	/// <returns>A <see cref="Book"/>.</returns>
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        if (await _ctx.Books.FindAsync(id) is not Book found)
          return null;

        return found;
    }

	/// <summary>
	/// Gets a list of all <see cref="Book"/> in the system.
	/// </summary>
	/// <returns>A list of <see cref="Book"/>.</returns>
    public async Task<List<Book>> GetBooksListAsync() =>
        await _ctx.Books.ToListAsync();

	/// <summary>
	/// Gets a list of <see cref="UserBookToRead"/>.
	/// </summary>
	/// <param name="userId">The id of the current <see cref="IdentityUser"/>.</param>
	/// <returns>A list of <see cref="UserBookToRead"/>.</returns>
    public async Task<List<UserBookToRead>> GetUserBookToReadByUserIdAsync(string userId) =>
        await _ctx.UserBookToReads.Where(b => b.ReaderId == userId).ToListAsync();

	/// <summary>
	/// Gets the current book the user is reading.
	/// </summary>
	/// <returns>A <see cref="UserBookToRead"/> for the <see cref="IdentityUser"/>.</returns>
	public async Task<UserBookToRead?> GetCurrentBookToReadAsync()
	{
		string userId = GetUserId();
		return await GetLatestUserBookByUserId(userId);
	}

	/// <summary>
	/// Sets the current book the user is reading to Order 0. Returns the
	/// following <see cref="UserBookToRead"/> based on Order.
	/// </summary>
	/// <returns>A <see cref="UserBookToRead"/> entity.</returns>
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

	/// <summary>
	/// Reads the list of <see cref="UserBookToRead"/> for a <see cref="IdentityUser"/>
	/// and randomizes a reading order.
	/// </summary>
	/// <returns>A list of <see cref="UserBookToRead"/>.</returns>
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

	/// <summary>
	/// Removes a <see cref="UserBookToRead"/> relationship by its Id.
	/// </summary>
	/// <param name="id">The primary id of the <see cref="UserBookToRead"/>.</param>
	/// <returns>The new list of <see cref="UserBookToRead"/> for the <see cref="IdentityUser"/>.</returns>
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
