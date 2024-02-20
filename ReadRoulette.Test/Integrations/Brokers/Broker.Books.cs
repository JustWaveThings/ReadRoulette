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

    public async Task<Book> GetBook(int id, Dictionary<string, string>? headers)
    {
        var book = await _brokerClient.GetAsync<Book>($"{BookRelativeUrl}/{id}", headers)
            ?? throw new Exception("Throwing exception to catch.");
        return book;
    }
}
