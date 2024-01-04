namespace Gamex.DTO;

public class LoginDTO
{
    public string Username { get; set; }
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