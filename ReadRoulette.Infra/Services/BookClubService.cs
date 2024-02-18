using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadRoulette.Domain;
using ReadRoulette.Persistence;

namespace ReadRoulette.Infra;

public class BookClubService(
    AppDbContext _ctx, 
    IHttpContextAccessor _httpCtx, 
    UserManager<IdentityUser> _userManager) : IBookClubService
{
    /// <summary>
	/// Gets the id of the current <see cref="IdentityUser"/>.
	/// </summary>
	/// <returns>A string id.</returns>
	/// <exception cref="Exception"></exception>
    private string GetUserId()
    {
        var currentUser = _httpCtx.HttpContext?.User ?? throw new Exception("A user was not present on the HttpContext.");
        var userId = _userManager.GetUserId(currentUser) ?? throw new Exception("A user was not present.");

        return userId;
    }
    
    public async Task<BookClub> CreateBookClubAsync(BookClub newBookClub)
    {
        string userId = GetUserId();
        var newId = string.Join("", Guid.NewGuid().ToString().Split('-'));

        newBookClub.Id = newId;
        newBookClub.UserId = userId;
        var result = _ctx.BookClubs.Add(newBookClub);
        await _ctx.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<bool> DeleteBookClubByIdAsync(string id)
    {
        if (await _ctx.BookClubs.FindAsync(id) is not BookClub found)
            return false;

        if (found.UserId != GetUserId())
            return false;

        _ctx.Remove(found);
        await _ctx.SaveChangesAsync();

        return true;
    }

    public async Task<BookClub?> GetBookClubByIdAsync(string id) =>
        await _ctx.BookClubs.FirstOrDefaultAsync(b => b.Id == id);

    public async Task<List<BookClub>> GetBookClubListAsync() =>
        await _ctx.BookClubs.ToListAsync();

    public async Task<List<BookClub>> GetBookClubsByOwnerIdAsync(string ownerId) =>
        await _ctx.BookClubs.Where(b => b.UserId == ownerId).ToListAsync();

    public async Task<List<BookClub>> GetBookClubsByCurrentUserAsync() =>
        await GetBookClubsByUserIdAsync(GetUserId());

    public async Task<List<BookClub>> GetBookClubsByUserIdAsync(string userId)
    {
        var ownedBookClubs = await GetBookClubsByOwnerIdAsync(userId);
        var userBookClubs = await _ctx.BookClubMembers
            .Where(b => b.UserId == userId)
            .Join(_ctx.BookClubs, bc => bc.BookClubId, b => b.Id, (_, b) => b)
            .ToListAsync();

        return Enumerable.Concat(ownedBookClubs, userBookClubs).ToList();
    }
}
