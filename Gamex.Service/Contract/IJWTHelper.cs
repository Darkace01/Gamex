namespace Gamex.Service.Contract
{
    public interface IJWTHelper
    {
        /// <summary>
        /// Generates a JWT token for the specified user with the given roles.
        /// </summary>
        /// <param name="user">The ApplicationUser object representing the user.</param>
        /// <param name="userRoles">The list of roles assigned to the user.</param>
        /// <returns>The generated JwtSecurityToken.</returns>
        JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> userRoles);

        /// <summary>
        /// Retrieves the ClaimsPrincipal from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token.</param>
        /// <returns>The ClaimsPrincipal object representing the user.</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}