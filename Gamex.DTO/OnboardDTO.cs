using Microsoft.AspNetCore.Http;

namespace Gamex.DTO;

public class UserProfileUpdateDTO
{
    public string DisplayName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public IFormFile ProfilePicture { get; set; } = null;
}

public class UserProfileDTO
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public string ProfilePicturePublicId { get; set; } = string.Empty;
}
