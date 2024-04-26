namespace Gamex.Models;
public class UserConfirmationCode: Entity
{
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMinutes(30);
    public string UserId { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public ApplicationUser User { get; set; } = null!;
}