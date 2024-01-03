using Microsoft.AspNetCore.Identity;

namespace Gamex.Models;

public class ApplicationUser: IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public Guid PictureId { get; set; }
    public Picture? Picture { get; set; } 
    public List<UserTournament> UserTournaments { get; set; } = new();
}
