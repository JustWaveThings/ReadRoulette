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
  public async Task<IActionResult> Post([FromBody] Book newBook)
  {
    var book = await _bookService.CreateBookAsync(newBook);
    return Ok(book);
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> Delete(int id)
  {
    var wasSuccessful = await _bookService.DeleteBookByIdAsync(id);
    return Ok(wasSuccessful);
  }
}
