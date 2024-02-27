using ReadRoulette.Persistence;

namespace ReadRoulette.Test;

[Collection(nameof(ApiTestCollection))]
public class BookTests(Broker _broker)
{
    private async Task AuthenticateUser(string email, string password)
    {
        await _broker.RegisterUser(email, password);
        await _broker.AuthUser(email, password);
    }

    private async Task<Book> SetupBook()
    {
        var book = new Book
        {
            Title = "ACreatedBook",
            Author = "Unknown"
        };

        return await _broker.CreateBook(book, null);
    }

    [Fact]
    public async Task GetAllBooks_Succeeds()
    {
        var result = await _broker.GetAllBooks(null);

        Assert.Multiple(() => 
        {
            Assert.NotEmpty(result);
            Assert.NotNull(result);
        });
    }

    [Fact]
    public async Task GetBookById_Succeeds()
    {
        var result = await _broker.GetBook(1, null);

        Assert.Multiple(() => 
        {
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Author1", result.Author);
        });
    }

    [Fact]
    public async Task GetBookById_WithNoBook_Fails()
    {
        await Assert.ThrowsAsync<Exception>(async () => 
            await _broker.GetBook(1000, null));
    }

    [Fact]
    public async Task CreateBook_Succeeds()
    {
        await AuthenticateUser("test4@gmail.com", "ASecretPw1!");

        var request = new Book
        {
            Title = "ABookToCreate",
            Author = "Unknown"
        };

        var response = await _broker.CreateBook(request, null);
        Assert.Multiple(() => {
            Assert.NotNull(response);
            Assert.IsType<Book>(response);
            Assert.NotEqual(0, response.Id);
            Assert.Equal("ABookToCreate", response.Title);
        });
    }

    [Fact]
    public async Task CreateBook_WhileUnauthorized_Fails()
    {
        // Ensure the client is logged out.
        _broker.Logout();

        var request = new Book
        {
            Title = "StrangeBook",
            Author = "AStranger"
        };

        await Assert.ThrowsAsync<Exception>(async () => await _broker.CreateBook(request, null));
    }

    [Fact]
    public async Task DeleteBook_Succeeds()
    {
        await AuthenticateUser("test6@gmail.com", "ASecretPw1!");

        var createdBook = await SetupBook();
        Assert.NotNull(createdBook);

        var response = await _broker.DeleteBook(createdBook.Id, null);
        Assert.True(response);
    }

    [Fact]
    public async Task DeleteBook_WithoutBook_Fails()
    {
        await AuthenticateUser("test7@gmail.com", "ASecretPw1!");

        await Assert.ThrowsAsync<Exception>(async () => await _broker.DeleteBook(1000, null));
    }

    [Fact]
    public async Task AddUserBook_Succeeds()
    {
        await AuthenticateUser("atestuser@yahoo.com", "ASecretPw1!");
        var createdBook = await SetupBook();

        var response = await _broker.AddUserBook(createdBook, null);
        Assert.Multiple(() => 
        {
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.IsType<List<UserBookToRead>>(response);
            Assert.Single(response);
            Assert.NotNull(response.First().ReaderId);
        });
    }

    [Fact]
    public async Task RemoveUserBook_Succeeds()
    {
        await AuthenticateUser("anewuser@yahoo.com", "ASecretPw1!");
        var createdBook = await SetupBook();

        var createdUserBook = await _broker.AddUserBook(createdBook, null);
        var response = await _broker.RemoveUserBook(createdUserBook.First().Id, null);
        Assert.Multiple(() => 
        {
            Assert.NotNull(response);
            Assert.Empty(response);
            Assert.IsType<List<UserBookToRead>>(response);
            Assert.NotEqual(createdUserBook.Count, response.Count);
        });
    }

    [Fact]
    public async Task RemoveUserBook_WithoutUserBook_ReturnsOriginalList()
    {
        await AuthenticateUser("asimpleuser@gmail.com", "ASecretPw1!");
        var createdBook = await SetupBook();

        var createdUserBook = await _broker.AddUserBook(createdBook, null);
        var response = await _broker.RemoveUserBook(1000, null);
        Assert.Multiple(() => 
        {
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.IsType<List<UserBookToRead>>(response);
            Assert.Equal(createdUserBook.Count, response.Count);
        });
    }

    [Fact]
    public async Task RandomizeBookList_Succeeds()
    {
        await AuthenticateUser("anothertestuser@yahoo.com", "ASecretPw1!");
        var createdBook1 = await SetupBook();
        var createdBook2 = await SetupBook();
        var createdBook3 = await SetupBook();
        var createdBook4 = await SetupBook();

        await _broker.AddUserBook(createdBook1, null);
        await _broker.AddUserBook(createdBook2, null);
        await _broker.AddUserBook(createdBook3, null);
        var createdUserBooks = await _broker.AddUserBook(createdBook4, null);

        Assert.Multiple(() =>
        {
            Assert.NotEmpty(createdUserBooks);
            Assert.Equal(4, createdUserBooks.Count);
            Assert.True(createdUserBooks.All(c => c.Order == 0));
        });

        var results = await _broker.RandomizeBookList(null);

        Assert.Multiple(() =>
        {
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(4, results.Count);
            Assert.False(results.All(c => c.Order == 0));
            
            var orderedResults = results.OrderBy(r => r.Order);
            Assert.True(orderedResults.First().Order <= orderedResults.Last().Order);
        });
    }

    [Fact]
    public async Task GetNextBook_Succeeds()
    {
        await AuthenticateUser("arandomuser@yahoo.com", "ASecretPw1!");
        var createdBook1 = await SetupBook();
        var createdBook2 = await SetupBook();
        var createdBook3 = await SetupBook();
        var createdBook4 = await SetupBook();

        await _broker.AddUserBook(createdBook1, null);
        await _broker.AddUserBook(createdBook2, null);
        await _broker.AddUserBook(createdBook3, null);
        var createdUserBooks = await _broker.AddUserBook(createdBook4, null);

        Assert.Multiple(() =>
        {
            Assert.NotEmpty(createdUserBooks);
            Assert.Equal(4, createdUserBooks.Count);
            Assert.True(createdUserBooks.All(c => c.Order == 0));
        });

        var firstBook = createdUserBooks.OrderBy(c => c.Order).First();

        var results = await _broker.GetNextBook(null);
        Assert.Multiple(() =>
        {
            Assert.NotNull(results);
            Assert.IsType<UserBookToRead>(results);
            Assert.NotEqual(firstBook.BookId, results.BookId);
        });
    }
}
