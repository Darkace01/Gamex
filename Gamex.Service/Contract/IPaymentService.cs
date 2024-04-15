
namespace Gamex.Service.Contract
{
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a payment transaction.
        /// </summary>
        /// <param name="paymentTransactionDTO">The payment transaction data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreatePaymentTransaction(PaymentTransactionCreateDTO paymentTransactionDTO, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a payment transaction.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePaymentTransaction(Guid transactionId);

        /// <summary>
        /// Gets a payment transaction by ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<PaymentTransactionDTO?> GetPaymentTransaction(Guid transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all payment transactions.
        /// </summary>
        /// <returns>An IQueryable of PaymentTransactionDTO representing the payment transactions.</returns>
        IQueryable<PaymentTransactionDTO> GetPaymentTransactions();

        /// <summary>
        /// Gets a payment transaction by reference.
        /// </summary>
        /// <param name="transactionReference">The reference of the transaction to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<PaymentTransactionDTO?> GetPaymentTransactionsByReference(string transactionReference, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all payment transactions by tournament ID.
        /// </summary>
        /// <param name="tournamentId">The ID of the tournament.</param>
        /// <returns>An IQueryable of PaymentTransactionDTO representing the payment transactions.</returns>
        IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByTournament(Guid tournamentId);

        /// <summary>
        /// Gets all payment transactions by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An IQueryable of PaymentTransactionDTO representing the payment transactions.</returns>
        IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByUser(string userId);

        /// <summary>
        /// Gets the balance of a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<decimal> GetUserBalance(string userId);

        /// <summary>
        /// Updates the status of a payment transaction by ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to update.</param>
        /// <param name="status">The new status of the transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdatePaymentTransactionStatus(Guid transactionId, Common.TransactionStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the status of a payment transaction by reference.
        /// </summary>
        /// <param name="transactionReference">The reference of the transaction to update.</param>
        /// <param name="status">The new status of the transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdatePaymentTransactionStatus(string transactionReference, Common.TransactionStatus status, CancellationToken cancellationToken = default);
    }
}