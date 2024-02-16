using Microsoft.EntityFrameworkCore;
using ReadRoulette.Domain;
using ReadRoulette.Persistence;

namespace ReadRoulette.Infra;

public class BookService(AppDbContext _ctx) : IBookService
{
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
}
