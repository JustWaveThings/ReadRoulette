using ReadRoulette.Persistence;

namespace ReadRoulette.Domain;

public interface IBookClubService
{
    public Task<List<BookClub>> GetBookClubListAsync();
    public Task<BookClub?> GetBookClubByIdAsync(string id);
    public Task<List<BookClub>> GetBookClubsByCurrentUserAsync();
    public Task<List<BookClub>> GetBookClubsByUserIdAsync(string userId);
    public Task<List<BookClub>> GetBookClubsByOwnerIdAsync(string ownerId);
    public Task<BookClub> CreateBookClubAsync(BookClub newBookClub);
    public Task<bool> DeleteBookClubByIdAsync(string id);
}
