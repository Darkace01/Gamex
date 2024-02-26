namespace Gamex.Service.Implementation;

public class PaymentService(GamexDbContext context) : IPaymentService
{
    private readonly GamexDbContext _context = context;

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

    public async Task UpdatePaymentTransactionStatus(Guid transactionId, Common.TransactionStatus status, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        if (paymentTransaction != null)
        {
            paymentTransaction.Status = (TransactionStatus)status;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    public async Task UpdatePaymentTransactionStatus(string transactionReference, Common.TransactionStatus status, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.TransactionReference == transactionReference);
        if (paymentTransaction != null)
        {
            paymentTransaction.Status = (TransactionStatus)status;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

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

    public async Task<decimal> GetUserBalance(string userId)
    {
        var userTransactions = await _context.PaymentTransactions.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        return userTransactions.Sum(x => x.Amount ?? 0);
    }

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
