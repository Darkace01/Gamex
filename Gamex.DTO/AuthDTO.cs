using System.ComponentModel.DataAnnotations;

namespace Gamex.DTO;

public class LoginDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

public class LoginResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long ExpiresIn { get; set; }
    public string TokenType { get; set; }

    public LoginResponseDTO(string accessToken, string refreshToken, long expiresIn, string tokenType)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
        TokenType = tokenType;
    }
}

public class RefreshTokenDTO
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}

public class RegisterDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string DisplayName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
}