namespace Gamex.Service.Implementation;

public class PaymentService(GamexDbContext context) : IPaymentService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Creates a new payment transaction.
    /// </summary>
    /// <param name="paymentTransactionDTO">The payment transaction data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task CreatePaymentTransaction(PaymentTransactionCreateDTO paymentTransactionDTO, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = new PaymentTransaction
        {
            UserId = paymentTransactionDTO.UserId,
            TournamentId = paymentTransactionDTO.TournamentId,
            Amount = paymentTransactionDTO.Amount,
            Status = (TransactionStatus)paymentTransactionDTO.Status,
            TransactionReference = paymentTransactionDTO.TransactionReference
        };

        _context.PaymentTransactions.Add(paymentTransaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the status of a payment transaction by transaction ID.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="status">The new transaction status.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdatePaymentTransactionStatus(Guid transactionId, Common.TransactionStatus status, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        if (paymentTransaction != null)
        {
            paymentTransaction.Status = (TransactionStatus)status;
            paymentTransaction.DateModified = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Updates the status of a payment transaction by transaction reference.
    /// </summary>
    /// <param name="transactionReference">The transaction reference.</param>
    /// <param name="status">The new transaction status.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdatePaymentTransactionStatus(string transactionReference, Common.TransactionStatus status, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.TransactionReference == transactionReference,cancellationToken);
        if (paymentTransaction != null)
        {
            paymentTransaction.Status = (TransactionStatus)status;
            paymentTransaction.DateModified = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Retrieves a payment transaction by transaction ID.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment transaction data transfer object.</returns>
    public async Task<PaymentTransactionDTO?> GetPaymentTransaction(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == transactionId, cancellationToken);
        if (paymentTransaction != null)
        {
            return new PaymentTransactionDTO
            {
                Id = paymentTransaction.Id,
                UserId = paymentTransaction.UserId,
                TournamentId = paymentTransaction.TournamentId,
                Amount = paymentTransaction.Amount,
                Status = (Common.TransactionStatus)paymentTransaction.Status,
                TransactionReference = paymentTransaction.TransactionReference
            };
        }
        return null;
    }

    /// <summary>
    /// Retrieves payment transactions by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>An IQueryable of payment transaction data transfer objects.</returns>
    public IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByUser(string userId)
    {
        var paymentTransactions = _context.PaymentTransactions.AsNoTracking().Where(x => x.UserId == userId);
        return paymentTransactions.Select(x => new PaymentTransactionDTO
        {
            Id = x.Id,
            UserId = x.UserId,
            TournamentId = x.TournamentId,
            Amount = x.Amount,
            Status = (Common.TransactionStatus)x.Status,
            TransactionReference = x.TransactionReference
        });
    }

    /// <summary>
    /// Retrieves payment transactions by tournament ID.
    /// </summary>
    /// <param name="tournamentId">The tournament ID.</param>
    /// <returns>An IQueryable of payment transaction data transfer objects.</returns>
    public IQueryable<PaymentTransactionDTO> GetPaymentTransactionsByTournament(Guid tournamentId)
    {
        var paymentTransactions = _context.PaymentTransactions.AsNoTracking().Where(x => x.TournamentId == tournamentId);
        return paymentTransactions.Select(x => new PaymentTransactionDTO
        {
            Id = x.Id,
            UserId = x.UserId,
            TournamentId = x.TournamentId,
            Amount = x.Amount,
            Status = (Common.TransactionStatus)x.Status,
            TransactionReference = x.TransactionReference
        });
    }

    /// <summary>
    /// Retrieves a payment transaction by transaction reference.
    /// </summary>
    /// <param name="transactionReference">The transaction reference.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment transaction data transfer object.</returns>
    public async Task<PaymentTransactionDTO?> GetPaymentTransactionsByReference(string transactionReference, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.AsNoTracking().FirstOrDefaultAsync(x => x.TransactionReference == transactionReference, cancellationToken);
        if (paymentTransaction != null)
        {
            return new PaymentTransactionDTO
            {
                Id = paymentTransaction.Id,
                UserId = paymentTransaction.UserId,
                TournamentId = paymentTransaction.TournamentId,
                Amount = paymentTransaction.Amount,
                Status = (Common.TransactionStatus)paymentTransaction.Status,
                TransactionReference = paymentTransaction.TransactionReference
            };
        }
        return null;
    }

    /// <summary>
    /// Retrieves all payment transactions.
    /// </summary>
    /// <returns>An IQueryable of payment transaction data transfer objects.</returns>
    public IQueryable<PaymentTransactionDTO> GetPaymentTransactions()
    {
        var paymentTransactions = _context.PaymentTransactions.AsNoTracking();
        return paymentTransactions.Select(x => new PaymentTransactionDTO
        {
            Id = x.Id,
            UserId = x.UserId,
            TournamentId = x.TournamentId,
            Amount = x.Amount,
            Status = (Common.TransactionStatus)x.Status,
            TransactionReference = x.TransactionReference
        });
    }

    /// <summary>
    /// Retrieves the balance of a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user's balance.</returns>
    public async Task<decimal> GetUserBalance(string userId)
    {
        var userTransactions = await _context.PaymentTransactions.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        return userTransactions.Sum(x => x.Amount ?? 0);
    }

    /// <summary>
    /// Deletes a payment transaction by transaction ID.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    public async Task DeletePaymentTransaction(Guid transactionId)
    {
        var paymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        if (paymentTransaction != null)
        {
            _context.PaymentTransactions.Remove(paymentTransaction);
            await _context.SaveChangesAsync();
        }
    }
}
