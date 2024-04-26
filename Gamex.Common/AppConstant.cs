namespace Gamex.Common;

public class AppConstant
{
    // Roles
    public const string PublicUserRole = "User";
    public const string AdminUserRole = "Admin";

    // Picture tags
    public const string ProfilePictureTag = "profile-picture";
    public const string PostPictureTag = "post-picture";
    public const string TournamentPictureTag = "tournament-picture";
    public const string TournamentCoverPictureTag = "tournament-cover-picture";

    //Files
    public const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    //
    //_ = Guid.TryParse("00000000-0000-0000-0000-000000000000", out var defualtTournamentId);
    public static Guid DefaultTournamentId = Guid.Parse("00000000-0000-0000-0000-000000000000");
}
