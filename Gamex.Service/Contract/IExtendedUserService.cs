
namespace Gamex.Service.Contract
{
    public interface IExtendedUserService
    {
        /// <summary>
        /// Gets the user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The ApplicationUser object representing the user, or null if not found.</returns>
        ApplicationUser? GetUserByName(string username);

        /// <summary>
        /// Gets the user by their username for profile.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The UserProfileDTO object representing the user's profile, or null if not found.</returns>
        UserProfileDTO? GetUserByNameForProfile(string username);
    }
}