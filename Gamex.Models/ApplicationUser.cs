﻿using Microsoft.AspNetCore.Identity;

namespace Gamex.Models;

public class ApplicationUser: IdentityUser
{
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
    public List<UserTournament> UserTournaments { get; set; } = new();
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
}
