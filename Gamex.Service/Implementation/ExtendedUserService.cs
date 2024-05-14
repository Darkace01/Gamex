using Gamex.Common;

namespace Gamex.Service.Implementation;

public class ExtendedUserService : IExtendedUserService
{
    private readonly GamexDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendedUserService"/> class.
    /// </summary>
    /// <param name="context">The GamexDbContext.</param>
    public ExtendedUserService(GamexDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The ApplicationUser object representing the user.</returns>
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

    /// <summary>
    /// Retrieves a user by their username for the user profile.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The UserProfileDTO object representing the user profile.</returns>
    public UserProfileDTO? GetUserByNameForProfile(string username)
    {
        var user = _context.Users
            .AsNoTracking()
            .Where(u => u.UserName == username)
            .Select(u => new
            {
                User = u,
                Picture = _context.Pictures.AsNoTracking().FirstOrDefault(p => p.Id == u.PictureId),
                Balance = _context.PaymentTransactions.AsNoTracking().Where(pt => pt.UserId == u.Id && pt.Status == Models.TransactionStatus.Success).Sum(pt => pt.Amount)
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
                _context.UserTournaments.Count(ut => ut.UserId == u.User.Id && ut.WaitList == true),
                _context.Posts.Count(post => post.UserId == u.User.Id),
                _context.Comments.Count(comment => comment.UserId == u.User.Id),
                u.User.EmailConfirmed
            ))
            .FirstOrDefault();

        return user;
    }

    public UserPublicProfileDTO? GetPublicUserByIdForProfile(string id)
    {
        var user = _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new
            {
                User = u,
                Picture = _context.Pictures.AsNoTracking().FirstOrDefault(p => p.Id == u.PictureId),
            })
            .Select(u => new UserPublicProfileDTO(
                u.User.FirstName,
                u.User.LastName,
                u.User.DisplayName,
                u.User.Email,
                u.User.PhoneNumber,
                u.Picture.FileUrl,
                u.Picture.PublicId,
                _context.UserTournaments.Count(ut => ut.UserId == u.User.Id && ut.WaitList == true),
                _context.Posts.Count(post => post.UserId == u.User.Id),
                _context.Comments.Count(comment => comment.UserId == u.User.Id),
                u.User.EmailConfirmed,
                _context.UserTournaments.Where(ut => ut.UserId == u.User.Id).Sum(ut => ut.Point ?? 0)
            ))
            .FirstOrDefault();

        return user;
    }

    /// <summary>
    /// Generates a user confirmation code.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The generated UserConfirmationCodeDTO object.</returns>
    public async Task<UserConfirmationCodeDTO> GenerateUserConfirmationCode(string userId)
    {
        var token = new UserConfirmationCode()
        {
            UserId = userId,
            Code = CommonHelpers.GenerateRandomNumbers(6).ToString(),
            ExpiryDate = DateTime.Now.AddMinutes(30),
        };

        _context.UserConfirmationCodes.Add(token);
        await _context.SaveChangesAsync();

        return new UserConfirmationCodeDTO
        {
            Id = token.Id,
            Code = token.Code,
            ExpiryDate = token.ExpiryDate,
            IsUsed = token.IsUsed,
            UserId = token.UserId,
        };
    }

    /// <summary>
    /// Verifies a user confirmation code.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="code">The confirmation code.</param>
    /// <returns>True if the code is valid and not expired; otherwise, false.</returns>
    public async Task<bool> VerifyUserConfirmationCode(string userId, string code)
    {
        var userConfirmationCode = await _context.UserConfirmationCodes.FirstOrDefaultAsync(x => x.Code == code && x.ExpiryDate > DateTime.Now && x.UserId == userId);
        if (userConfirmationCode is null)
        {
            return false;
        }

        userConfirmationCode.IsUsed = true;
        userConfirmationCode.DateModified = DateTime.Now;
        _context.UserConfirmationCodes.Update(userConfirmationCode);

        await _context.SaveChangesAsync();
        return true;
    }
}
