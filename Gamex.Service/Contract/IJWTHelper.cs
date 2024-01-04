namespace Gamex.Service.Contract
{
    public interface IJWTHelper
    {
        JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> userRoles);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}