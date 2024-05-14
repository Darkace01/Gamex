using Microsoft.AspNetCore.Identity;

namespace Gamex.Models;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// User first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// User last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// User display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
    /// <summary>
    /// Profile Picture Id
    /// </summary>
    public Guid PictureId { get; set; }
    /// <summary>
    /// Profile Picture for user
    /// </summary>
    public Picture? Picture { get; set; }
    /// <summary>
    /// List of tournaments user is part of
    /// </summary>
    public ICollection<UserTournament> UserTournaments { get; set; } = new List<UserTournament>();
    /// <summary>
    /// Refresh token for user
    /// </summary>
    public string? RefreshToken { get; set; }
    /// <summary>
    /// Time for refresh token to expire
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }
    /// <summary>
    /// User has signed up with google
    /// </summary>
    public bool ExternalAuthInWithGoogle { get; set; }
    /// <summary>
    /// List of users blog post
    /// </summary>
    public ICollection<Post> Posts { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = default!;
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = default!;
    public ICollection<MatchUser> MatchUsers { get; set; } = default!;
}
