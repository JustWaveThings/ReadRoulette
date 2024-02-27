using ReadRoulette.Persistence;

namespace ReadRoulette.Test;

public partial class Broker
{
    private const string BookRelativeUrl = "/api/books";

    public async ValueTask<List<Book>> GetAllBooks(Dictionary<string, string>? headers)
    {
        var books = await _brokerClient.GetAsync<List<Book>>($"{BookRelativeUrl}", headers) 
            ?? throw new Exception("Throwing exception to catch.");
        return books;
    }

    public async Task<Book?> GetBook(int id, Dictionary<string, string>? headers)
    {
        var book = await _brokerClient.GetAsync<Book>($"{BookRelativeUrl}/{id}", headers)
            ?? throw new Exception("Throwing exception to catch.");
        return book;
    }

    public async Task<Book> CreateBook(Book newBook, Dictionary<string, string>? headers)
    {
        var book = await _brokerClient.PostAsync<Book, Book>($"{BookRelativeUrl}", newBook, headers)
            ?? throw new Exception("Throwing exception to catch.");

        return book;
    }

    public async Task<bool> DeleteBook(int id, Dictionary<string, string>? headers)
    {
        var result = await _brokerClient.DeleteAsync<bool>($"{BookRelativeUrl}/{id}", headers);
        if (!result) throw new Exception("Throwing exception to catch.");
        return result;
    }

    public async Task<List<UserBookToRead>> AddUserBook(Book book, Dictionary<string, string>? headers)
    {
        var result = await _brokerClient.PostAsync<Book, List<UserBookToRead>>($"{BookRelativeUrl}/user-book", book, headers)
            ?? throw new Exception("Throwing exception to catch.");

        return result;
    }

    public async Task<List<UserBookToRead>?> RemoveUserBook(int id, Dictionary<string, string>? headers)
    {
        return await _brokerClient.DeleteAsync<List<UserBookToRead>>($"{BookRelativeUrl}/user-book/{id}", headers)
            ?? throw new Exception("Throwing exception to catch.");
    }

    public async Task<List<UserBookToRead>?> RandomizeBookList(Dictionary<string, string>? headers)
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        return await _brokerClient.PutAsync<object, List<UserBookToRead>>($"{BookRelativeUrl}/user-book", null, headers);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    public async Task<UserBookToRead?> GetNextBook(Dictionary<string, string>? headers)
    {
        return await _brokerClient.GetAsync<UserBookToRead>($"{BookRelativeUrl}/user-book", headers)
            ?? throw new Exception("Throwing exception to catch.");
    }
}
