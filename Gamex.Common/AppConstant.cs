namespace Gamex.Common;

public static class AppConstant
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
}
