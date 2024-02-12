namespace Gamex.Service.Implementation;

public class ExtendedUserService(GamexDbContext context) : IExtendedUserService
{
    private readonly GamexDbContext _context = context;

    public ApplicationUser? GetUserByName(string username)
    {
        return _context.Users.AsNoTracking().Include(x => x.Picture).FirstOrDefault(u => u.UserName == username);
    }

    public UserProfileDTO? GetUserByNameForProfile(string username)
    {
        var user = _context.Users.AsNoTracking()
            .Include(x => x.Picture)
            .Include(x => x.Posts)
            .Include(x => x.Comments)
            .Include(x => x.UserTournaments)
            .FirstOrDefault(u => u.UserName == username);

        if (user is null)
        {
            return null;
        }

        return new UserProfileDTO(user.FirstName, user.LastName, user.DisplayName, user.Email, user.PhoneNumber, user.Picture?.FileUrl ?? string.Empty, user.Picture?.PublicId ?? string.Empty, 0, user.UserTournaments.Count, user.Posts.Count, user.Comments.Count);
    }
}
