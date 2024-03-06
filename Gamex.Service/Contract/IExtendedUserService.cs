

namespace Gamex.Service.Contract
{
    public interface IExtendedUserService
    {
        /// <summary>
        /// Generates a user confirmation code.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The generated UserConfirmationCodeDTO object.</returns>
        Task<UserConfirmationCodeDTO> GenerateUserConfirmationCode(string userId);

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
        /// <summary>
        /// Verifies a user confirmation code.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="code">The confirmation code.</param>
        /// <returns>True if the code is valid and not expired; otherwise, false.</returns>
        Task<bool> VerifyUserConfirmationCode(string userId, string code);
    }
}