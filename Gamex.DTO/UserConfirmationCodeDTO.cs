namespace Gamex.DTO;
public class UserConfirmationCodeDTO
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; } = false;
    public string UserId { get; set; } = string.Empty;
}
