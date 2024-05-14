namespace Gamex.DTO;

public class UserPublicProfileDTO
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
    public int ActiveTournaments { get; set; }
    public int Post { get; set; }
    public int Comments { get; set; }
    public bool ConfirmedEmail { get; set; }
    public int TotalPoints { get; set; }

    public UserPublicProfileDTO()
    {
    }

    public UserPublicProfileDTO(string firstName, string lastName, string displayName, string email, string phoneNumber, string profilePictureUrl, string profilePicturePublicId, bool confirmedEmail)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePictureUrl = profilePictureUrl;
        ProfilePicturePublicId = profilePicturePublicId;
        ConfirmedEmail = confirmedEmail;
    }

    public UserPublicProfileDTO(string firstName, string lastName, string displayName, string email, string phoneNumber, string profilePictureUrl, string profilePicturePublicId, int activeTournaments, int post, int comments, bool confirmedEmail)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePictureUrl = profilePictureUrl;
        ProfilePicturePublicId = profilePicturePublicId;
        ActiveTournaments = activeTournaments;
        Post = post;
        Comments = comments;
        ConfirmedEmail = confirmedEmail;
    }

    public UserPublicProfileDTO(string firstName, string lastName, string displayName, string email, string phoneNumber, string profilePictureUrl, string profilePicturePublicId, int activeTournaments, int post, int comments, bool confirmedEmail, int totalPoints)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePictureUrl = profilePictureUrl;
        ProfilePicturePublicId = profilePicturePublicId;
        ActiveTournaments = activeTournaments;
        Post = post;
        Comments = comments;
        ConfirmedEmail = confirmedEmail;
        TotalPoints = totalPoints;
    }
}
