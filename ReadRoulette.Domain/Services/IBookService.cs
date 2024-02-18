using ReadRoulette.Persistence;

namespace ReadRoulette.Domain;

public interface IBookService
{
  public Task<List<Book>> GetBooksListAsync();
  public Task<Book?> GetBookByIdAsync(int id);
  public Task<Book?> CreateBookAsync(Book newBook);
  public Task<bool> DeleteBookByIdAsync(int id);

  public Task<List<UserBookToRead>> AddBookToReadListAsync(Book newBook);
  public Task<List<UserBookToRead>> RemoveBookFromReadListAsync(int id);
  public Task<List<UserBookToRead>> RandomizeBookListAsync();
  public Task<UserBookToRead?> GetNextBookToReadAsync();
}
