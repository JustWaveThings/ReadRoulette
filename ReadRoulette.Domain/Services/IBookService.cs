using ReadRoulette.Persistence;

namespace ReadRoulette.Domain;

public interface IBookService
{
  public Task<List<Book>> GetBooksListAsync();
  public Task<Book?> GetBookByIdAsync(int id);
  public Task<Book?> CreateBookAsync(Book newBook);
  public Task<bool> DeleteBookByIdAsync(int id);
}
