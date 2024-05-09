namespace Gamex.Test.UnitTest;

public class RoundServiceTest : TestBase
{
    [Fact]
    public async Task CreateRound_ShouldCreateRound()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateRound_ShouldCreateRound));
        var roundService = MockRoundService(dbContext);
        TournamentRoundCreateDTO newRound = new()
        {
            Name = "Test Round",
            Description = "Test Round Description",
            TournamentId = Guid.NewGuid()
        };

        // Act
        await roundService.CreateRound(newRound);

        // Assert
        var roundInDb = dbContext.TournamentRounds.FirstOrDefault();
        Assert.NotNull(roundInDb);
    }

    [Fact]
    public async Task UpdateRound_ShouldUpdateRound()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateRound_ShouldUpdateRound));
        var roundService = MockRoundService(dbContext);
        var roundToUpdate = dbContext.TournamentRounds.FirstOrDefault();
        TournamentRoundUpdateDTO updatedRound = new()
        {
            Id = roundToUpdate.Id,
            Name = "Updated Round",
            Description = "Updated Round Description",
            TournamentId = roundToUpdate.TournamentId
        };

        // Act
        var (updated, message) = await roundService.UpdateRound(updatedRound);

        // Assert
        var updatedRoundInDb = dbContext.TournamentRounds.FirstOrDefault(r => r.Id == roundToUpdate.Id);
        Assert.True(updated);
        Assert.Equal(updatedRound.Name, updatedRoundInDb.Name);
    }

    [Fact]
    public async Task DeleteRound_ShouldDeleteRound()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteRound_ShouldDeleteRound));
        var roundService = MockRoundService(dbContext);
        var roundToDelete = dbContext.TournamentRounds.FirstOrDefault();

        // Act
        var (deleted, message) = await roundService.DeleteRound(roundToDelete.Id);

        // Assert
        var deletedRoundInDb = dbContext.TournamentRounds.FirstOrDefault(r => r.Id == roundToDelete.Id);
        Assert.True(deleted);
        Assert.Null(deletedRoundInDb);
    }

    [Fact]
    public void GetAllRoundsByTournamentId_ShouldReturnRounds()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllRoundsByTournamentId_ShouldReturnRounds));
        var roundService = MockRoundService(dbContext);
        var tournamentId = dbContext.Tournaments.FirstOrDefault().Id;

        // Act
        var rounds = roundService.GetAllRoundsByTournamentId(tournamentId);

        // Assert
        Assert.NotEmpty(rounds);
    }

    [Fact]
    public void GetRoundById_ShouldReturnRound()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetRoundById_ShouldReturnRound));
        var roundService = MockRoundService(dbContext);
        var roundId = dbContext.TournamentRounds.FirstOrDefault().Id;

        // Act
        var round = roundService.GetRoundById(roundId);

        // Assert
        Assert.NotNull(round);
    }

    [Fact]
    public void GetAllRounds_ShouldReturnRounds()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllRounds_ShouldReturnRounds));
        var roundService = MockRoundService(dbContext);

        // Act
        var rounds = roundService.GetAllRounds();

        // Assert
        Assert.NotEmpty(rounds);
    }

    #region Helpers
    private IRoundService MockRoundService(GamexDbContext context)
    {
        return new RoundService(context);
    }
    #endregion
}
