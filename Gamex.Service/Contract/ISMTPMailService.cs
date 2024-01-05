namespace Gamex.Service.Contract
{
    public interface ISMTPMailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
    }
}