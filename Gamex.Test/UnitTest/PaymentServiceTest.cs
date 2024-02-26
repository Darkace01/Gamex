using Gamex.Common;

namespace Gamex.Test.UnitTest;

public class PaymentServiceTest : TestBase
{
    [Fact]
    public async Task GetPaymentTransaction_ShouldReturnPaymentTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPaymentTransaction_ShouldReturnPaymentTransaction));
        var paymentService = MockPaymentService(dbContext);
        var paymentTransactionToGet = dbContext.PaymentTransactions.First();

        // Act
        var paymentTransaction = await paymentService.GetPaymentTransaction(paymentTransactionToGet.Id);

        // Assert
        Assert.NotNull(paymentTransaction);
        Assert.Equal(paymentTransactionToGet.Id, paymentTransaction.Id);
    }

    [Fact]
    public async Task GetPaymentTransactionsForUser_ShouldReturnPaymentTransactions()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPaymentTransactionsForUser_ShouldReturnPaymentTransactions));
        var paymentService = MockPaymentService(dbContext);
        var userId = dbContext.Users.First().Id;

        // Act
        var paymentTransactions = await paymentService.GetPaymentTransactions(userId).ToListAsync();

        // Assert
        Assert.NotNull(paymentTransactions);
        //Assert.Equal(dbContext.PaymentTransactions.Count(), paymentTransactions.Count());
    }

    [Fact]
    public async Task GetPaymentTransactionsForTournament_ShouldReturnPaymentTransactions()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPaymentTransactionsForTournament_ShouldReturnPaymentTransactions));
        var paymentService = MockPaymentService(dbContext);
        var tournamentId = dbContext.Tournaments.First().Id;

        // Act
        var paymentTransactions = await paymentService.GetPaymentTransactions(tournamentId).ToListAsync();

        // Assert
        Assert.NotNull(paymentTransactions);
    }

    [Fact]
    public async Task GetPaymentTransactions_ShouldReturnPaymentTransactions()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPaymentTransactions_ShouldReturnPaymentTransactions));
        var paymentService = MockPaymentService(dbContext);

        // Act
        var paymentTransactions = await paymentService.GetPaymentTransactions().ToListAsync();

        // Assert
        Assert.NotNull(paymentTransactions);
        //Assert.Equal(dbContext.PaymentTransactions.Count(), paymentTransactions.Count());
    }

    [Fact]
    public async Task GetUserBalance_ShouldReturnUserBalance()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetUserBalance_ShouldReturnUserBalance));
        var paymentService = MockPaymentService(dbContext);
        var userId = dbContext.Users.First().Id;

        // Act
        var userBalance = await paymentService.GetUserBalance(userId);

        // Assert
        Assert.Equal(300, userBalance);
    }

    [Fact]
    public async Task CreatePaymentTransaction_ShouldCreatePaymentTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreatePaymentTransaction_ShouldCreatePaymentTransaction));
        var paymentService = MockPaymentService(dbContext);
        var userId = dbContext.Users.First().Id;
        var tournamentId = dbContext.Tournaments.First().Id;

        var paymentTransaction = new PaymentTransactionCreateDTO
        {
            UserId = userId,
            TournamentId = tournamentId,
            Amount = 100,
            Status = Common.TransactionStatus.Pending,
            TransactionReference = CommonHelpers.GenerateRandomString(20)
        };

        // Act
        await paymentService.CreatePaymentTransaction(paymentTransaction);

        // Assert
        var createdPaymentTransaction = dbContext.PaymentTransactions.FirstOrDefault(x => x.TransactionReference == paymentTransaction.TransactionReference);
        Assert.NotNull(createdPaymentTransaction);
        Assert.Equal(paymentTransaction.UserId, createdPaymentTransaction.UserId);
    }

    [Fact]
    public async Task UpdatePaymentTransactionStatus_ShouldUpdatePaymentTransactionStatus()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdatePaymentTransactionStatus_ShouldUpdatePaymentTransactionStatus));
        var paymentService = MockPaymentService(dbContext);
        var paymentTransaction = dbContext.PaymentTransactions.First();
        var newStatus = Common.TransactionStatus.Success;

        // Act
        await paymentService.UpdatePaymentTransactionStatus(paymentTransaction.Id, newStatus);

        // Assert
        var updatedPaymentTransaction = dbContext.PaymentTransactions.FirstOrDefault(x => x.Id == paymentTransaction.Id);
        Assert.NotNull(updatedPaymentTransaction);
        Assert.Equal(newStatus, (Common.TransactionStatus)updatedPaymentTransaction.Status);
    }

    [Fact]
    public async Task DeletePaymentTransaction_ShouldDeletePaymentTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeletePaymentTransaction_ShouldDeletePaymentTransaction));
        var paymentService = MockPaymentService(dbContext);
        var paymentTransactionToDelete = dbContext.PaymentTransactions.First();

        // Act
        await paymentService.DeletePaymentTransaction(paymentTransactionToDelete.Id);

        // Assert
        var deletedPaymentTransactionInDb = dbContext.PaymentTransactions.FirstOrDefault(x => x.Id == paymentTransactionToDelete.Id);
        Assert.Null(deletedPaymentTransactionInDb);
    }

    #region Helpers
    private PaymentService MockPaymentService(GamexDbContext dbContext)
    {
        return new PaymentService(dbContext);
    }
    #endregion
}
