namespace Gamex.Test.UnitTest;

public class MatchServiceTest : TestBase
{
    [Fact]
    public async Task CreateMatch_ShouldCreateMatch()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateMatch_ShouldCreateMatch));
        var matchService = MockMatchService(dbContext);
        var round = dbContext.TournamentRounds.FirstOrDefault();
        MatchCreateDTO newMatch = new()
        {
            Name = "Test Match",
            RoundId = round.Id
        };

        // Act
        var (created, message) = await matchService.CreateMatch(newMatch);

        // Assert
        Assert.True(created);
        var matchInDb = dbContext.RoundMatches.FirstOrDefault();
        Assert.NotNull(matchInDb);
    }

    [Fact]
    public async Task UpdateMatch_ShouldUpdateMatch()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateMatch_ShouldUpdateMatch));
        var matchService = MockMatchService(dbContext);
        var matchToUpdate = dbContext.RoundMatches.FirstOrDefault();
        MatchUpdateDTO updatedMatch = new()
        {
            Id = matchToUpdate.Id,
            Name = "Updated Match"
        };

        // Act
        var (updated, message) = await matchService.UpdateMatch(updatedMatch);

        // Assert
        var updatedMatchInDb = dbContext.RoundMatches.FirstOrDefault(m => m.Id == matchToUpdate.Id);
        Assert.True(updated);
        Assert.Equal(updatedMatch.Name, updatedMatchInDb.Name);
    }

    [Fact]
    public async Task DeleteMatch_ShouldDeleteMatch()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteMatch_ShouldDeleteMatch));
        var matchService = MockMatchService(dbContext);
        var matchToDelete = dbContext.RoundMatches.FirstOrDefault();

        // Act
        var (deleted, message) = await matchService.DeleteMatch(matchToDelete.Id);

        // Assert
        var deletedMatchInDb = dbContext.RoundMatches.FirstOrDefault(m => m.Id == matchToDelete.Id);
        Assert.True(deleted);
        Assert.Null(deletedMatchInDb);
    }

    //IQueryable<MatchDTO> GetAllMatches();
    //IQueryable<MatchDTO> GetAllMatchesByRoundId(Guid roundId);
    //MatchDTO? GetMatchById(Guid id);

    [Fact]
    public void GetAllMatches_ShouldReturnAllMatches()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllMatches_ShouldReturnAllMatches));
        var matchService = MockMatchService(dbContext);

        // Act
        var matches = matchService.GetAllMatches();

        // Assert
        Assert.Equal(3, matches.Count());
    }

    [Fact]
    public void GetAllMatchesByRoundId_ShouldReturnMatchesByRoundId()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllMatchesByRoundId_ShouldReturnMatchesByRoundId));
        var matchService = MockMatchService(dbContext);
        var round = dbContext.TournamentRounds.FirstOrDefault(x => x.Name == "Group Stage");

        // Act
        var matches = matchService.GetAllMatchesByRoundId(round.Id);

        // Assert
        Assert.Equal(2, matches.Count());
    }

    [Fact]
    public void GetMatchById_ShouldReturnMatchById()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetMatchById_ShouldReturnMatchById));
        var matchService = MockMatchService(dbContext);
        var match = dbContext.RoundMatches.FirstOrDefault();

        // Act
        var matchInDb = matchService.GetMatchById(match.Id);

        // Assert
        Assert.NotNull(matchInDb);
        Assert.Equal(match.Id, matchInDb.Id);
    }
    #region Helpers
    private IMatchService MockMatchService(GamexDbContext context)
    {
        return new MatchService(context);
    }
    #endregion
}
