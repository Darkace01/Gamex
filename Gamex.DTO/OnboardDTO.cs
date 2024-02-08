using Microsoft.AspNetCore.Http;

namespace Gamex.DTO;

public class UserProfileUpdateDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public IFormFile ProfilePicture { get; set; } = null;
}

public class UserProfileDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    private string _displayName = string.Empty;
    public string DisplayName
    {
        get
        {
            return string.IsNullOrWhiteSpace(_displayName) ? $"{FirstName} {LastName}" : _displayName;
        }
        set => _displayName = value;
    }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public string ProfilePicturePublicId { get; set; } = string.Empty;

    public UserProfileDTO()
    {
    }

    public UserProfileDTO(string firstName,string lastName,string displayName, string email, string phoneNumber, string profilePictureUrl, string profilePicturePublicId)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePictureUrl = profilePictureUrl;
        ProfilePicturePublicId = profilePicturePublicId;
    }
}
