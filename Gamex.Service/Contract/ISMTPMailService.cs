namespace Gamex.Service.Contract
{
    public interface ISMTPMailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The content of the email.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the email was sent successfully; otherwise, false.</returns>
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
    }
}