namespace Gamex.Service.Implementation;

public class ExtendedUserService(GamexDbContext context) : IExtendedUserService
{
    private readonly GamexDbContext _context = context;

    public ApplicationUser? GetUserByName(string username)
    {
        return _context.Users.AsNoTracking().Include(x => x.Picture).FirstOrDefault(u => u.UserName == username);
    }
}
