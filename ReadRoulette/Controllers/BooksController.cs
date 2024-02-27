using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadRoulette.Domain;
using ReadRoulette.Persistence;

namespace ReadRoulette;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService _bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _bookService.GetBooksListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] Book newBook) =>
        Ok(await _bookService.CreateBookAsync(newBook));

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await _bookService.DeleteBookByIdAsync(id));

    [HttpPost("user-book")]
    [Authorize]
    public async Task<IActionResult> AddUserBook([FromBody] Book newBook) =>
        Ok(await _bookService.AddBookToReadListAsync(newBook));

    [HttpDelete("user-book/{id}")]
    [Authorize]
    public async Task<IActionResult> RemoveUserBook(int id) =>
        Ok(await _bookService.RemoveBookFromReadListAsync(id));

    [HttpPut("user-book")]
    public async Task<IActionResult> RandomizeBookList() =>
        Ok(await _bookService.RandomizeBookListAsync());

    [HttpGet("user-book")]
    public async Task<IActionResult> GetNextBook() =>
        Ok(await _bookService.GetNextBookToReadAsync());
}
