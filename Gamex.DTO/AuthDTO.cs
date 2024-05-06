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
    public string Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long ExpiresIn { get; set; }
    public string TokenType { get; set; }
    public UserMiniDTO User { get; set; }

    public LoginResponseDTO(string id, string accessToken, string refreshToken, long expiresIn, string tokenType, UserMiniDTO user)
    {
        Id = id;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
        TokenType = tokenType;
        User = user;
    }
}

public class UserMiniDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }

    public UserMiniDTO(string email, string image, string firstName, string lastName)
    {
        Email = email;
        Image = image;
        Name = $"{firstName} {lastName}";
        FirstName = firstName;
        LastName = lastName;
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
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
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

public class ExternalAuthDTO
{
    [Required]
    public string Token { get; set; }
}

public class ChangePasswordDTO
{
    [Required]
    public string CurrentPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}

public class ForgotPasswordDTO
{
    [Required]
    public string Email { get; set; }
}

public class ResetPasswordDTO
{
    [Required]
    public string Code { get; set; }
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    [Required]
    public string Email { get; set; }
}

public class RequestConfirmEmailDTO
{
    [Required]
    public string Email { get; set; }
}
public class ConfirmEmailDTO
{
    [Required]
    public string Code { get; set; }
    [Required]
    public string Email { get; set; }
}