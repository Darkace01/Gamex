namespace Gamex.Service.Implementation;

public class ExtendedUserService(GamexDbContext context) : IExtendedUserService
{
    private readonly GamexDbContext _context = context;

    public ApplicationUser? GetUserByName(string username)
    {
        var user = from u in _context.Users
                   where u.UserName == username
                   join p in _context.Pictures on u.PictureId equals p.Id into pic
                   select new ApplicationUser
                   {
                       Id = u.Id,
                       UserName = u.UserName,
                       Email = u.Email,
                       PhoneNumber = u.PhoneNumber,
                       FirstName = u.FirstName,
                       LastName = u.LastName,
                       Picture = pic.FirstOrDefault(),
                       AccessFailedCount = u.AccessFailedCount,
                       ConcurrencyStamp = u.ConcurrencyStamp,
                       EmailConfirmed = u.EmailConfirmed,
                       LockoutEnabled = u.LockoutEnabled,
                       DisplayName = u.DisplayName,
                       Comments = u.Comments,
                       ExternalAuthInWithGoogle = u.ExternalAuthInWithGoogle,
                       LockoutEnd = u.LockoutEnd,
                       NormalizedEmail = u.NormalizedEmail,
                       NormalizedUserName = u.NormalizedUserName,
                       PasswordHash = u.PasswordHash,
                       PaymentTransactions = u.PaymentTransactions,
                       PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                       PictureId = u.PictureId,
                       RefreshToken = u.RefreshToken,
                       RefreshTokenExpiryTime = u.RefreshTokenExpiryTime,
                       SecurityStamp = u.SecurityStamp,
                       TwoFactorEnabled = u.TwoFactorEnabled,
                       Posts = u.Posts,
                       UserTournaments = u.UserTournaments,
                   };
        return user.FirstOrDefault();
    }

    public UserProfileDTO? GetUserByNameForProfile(string username)
    {

        var user = _context.Users
            .AsNoTracking()
            .Where(u => u.UserName == username)
            .Select(u => new
            {
                User = u,
                Picture = _context.Pictures.AsNoTracking().FirstOrDefault(p => p.Id == u.PictureId),
                Balance = _context.PaymentTransactions.AsNoTracking().Where(pt => pt.UserId == u.Id && pt.Status == TransactionStatus.Success).Sum(pt => pt.Amount)
            })
            .Select(u => new UserProfileDTO(
                u.User.FirstName,
                u.User.LastName,
                u.User.DisplayName,
                u.User.Email,
                u.User.PhoneNumber,
                u.Picture.FileUrl,
                u.Picture.PublicId,
                u.Balance,
                _context.UserTournaments.Count(ut => ut.UserId == u.User.Id),
                _context.Posts.Count(post => post.UserId == u.User.Id),
                _context.Comments.Count(comment => comment.UserId == u.User.Id)
            ))
            .FirstOrDefault();

        return user;
    }
}
