namespace ReadRoulette.Test;

[Collection(nameof(ApiTestCollection))]
public class BookTests(Broker broker)
{
    [Fact]
    public async Task GetAllBooks_Succeeds()
    {
        var result = await broker.GetAllBooks(null);

        Assert.Multiple(() => 
        {
            Assert.NotEmpty(result);
            Assert.NotNull(result);
        });
    }

    [Fact]
    public async Task GetBookById_Succeeds()
    {
        var result = await broker.GetBook(1, null);
        Console.WriteLine($"{result.Id} - {result.Title} - {result.Author}");

        Assert.Multiple(() => 
        {
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Author1", result.Author);
        });
    }

    [Fact]
    public async Task CreateBook_Succeeds()
    {
        var users = broker.DbContext.Users.ToList();
        var response = await broker.AuthUser("test@gmail.com", "ASecretPw!");
        Assert.NotNull(response);
    }
}
