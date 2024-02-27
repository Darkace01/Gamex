
namespace Gamex.Service.Contract
{
    public interface IPaymentService
    {
        Task CreatePaymentTransaction(PaymentTransactionCreateDTO paymentTransactionDTO, CancellationToken cancellationToken = default);
        Task DeletePaymentTransaction(Guid transactionId);
        Task<PaymentTransactionDTO?> GetPaymentTransaction(Guid transactionId, CancellationToken cancellationToken = default);
        IQueryable<PaymentTransactionDTO> GetPaymentTransactions();
        Task<PaymentTransactionDTO?> GetPaymentTransactionsByReference(string transactionReference, CancellationToken cancellationToken = default);
        IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByTournament(Guid tournamentId);
        IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByUser(string userId);
        Task<decimal> GetUserBalance(string userId);
        Task UpdatePaymentTransactionStatus(Guid transactionId, Common.TransactionStatus status, CancellationToken cancellationToken = default);
        Task UpdatePaymentTransactionStatus(string transactionReference, Common.TransactionStatus status, CancellationToken cancellationToken = default);
    }
}