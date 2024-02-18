using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadRoulette.Domain;
using ReadRoulette.Persistence;

namespace ReadRoulette;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookClubController(IBookClubService _bookClubService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _bookClubService.GetBookClubListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _bookClubService.GetBookClubByIdAsync(id);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUserBookClubs() =>
        Ok(await _bookClubService.GetBookClubsByCurrentUserAsync());

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBookClubs(string userId) =>
        Ok(await _bookClubService.GetBookClubsByUserIdAsync(userId));

    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetOwnerBookClubs(string ownerId) =>
        Ok(await _bookClubService.GetBookClubsByOwnerIdAsync(ownerId));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BookClub newBookClub) =>
        Ok(await _bookClubService.CreateBookClubAsync(newBookClub));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var wasSuccessful = await _bookClubService.DeleteBookClubByIdAsync(id);
        if (!wasSuccessful)
            return NotFound();

        return Ok();
    }

}
