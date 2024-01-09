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

    public UserProfileDTO()
    {
    }

    public UserProfileDTO(string displayName, string email, string phoneNumber, string profilePictureUrl, string profilePicturePublicId)
    {
        DisplayName = displayName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePictureUrl = profilePictureUrl;
        ProfilePicturePublicId = profilePicturePublicId;
    }
}
