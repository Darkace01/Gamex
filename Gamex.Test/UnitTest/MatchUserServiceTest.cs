namespace Gamex.Test.UnitTest;

public class MatchUserServiceTest : TestBase
{
    [Fact]
    public async Task CreateMatchUser_ShouldCreateMatchUser()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateMatchUser_ShouldCreateMatchUser));
        var matchUserService = MockMatchUserService(dbContext);
        var match = dbContext.RoundMatches.FirstOrDefault();
        var userInTournament = dbContext.UserTournaments.FirstOrDefault();
        MatchUserCreateDTO newMatchUser = new()
        {
            MatchId = match.Id,
            UserId = userInTournament.UserId
        };

        // Act
        var (created, message) = await matchUserService.CreateMatchUser(newMatchUser);

        // Assert
        Assert.True(created);
        var matchUserInDb = dbContext.MatchUsers.FirstOrDefault();
        Assert.NotNull(matchUserInDb);
    }

    [Fact]
    public async Task UpdateMatchUser_ShouldUpdateMatchUser()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateMatchUser_ShouldUpdateMatchUser));
        var matchUserService = MockMatchUserService(dbContext);
        var matchUserToUpdate = dbContext.MatchUsers.FirstOrDefault();
        MatchUserUpdateDTO updatedMatchUser = new()
        {
            Id = matchUserToUpdate.Id,
            Points = 100
        };

        // Act
        var (updated, message) = await matchUserService.UpdateMatchUser(updatedMatchUser);

        // Assert
        var updatedMatchUserInDb = dbContext.MatchUsers.FirstOrDefault(m => m.Id == matchUserToUpdate.Id);
        Assert.True(updated);
        Assert.Equal(updatedMatchUser.Points, updatedMatchUserInDb.Point);
    }

    [Fact]
    public async Task DeleteMatchUser_ShouldDeleteMatchUser()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteMatchUser_ShouldDeleteMatchUser));
        var matchUserService = MockMatchUserService(dbContext);
        var matchUserToDelete = dbContext.MatchUsers.FirstOrDefault();

        // Act
        var (deleted, message) = await matchUserService.DeleteMatchUser(matchUserToDelete.Id);

        // Assert
        var deletedMatchUserInDb = dbContext.MatchUsers.FirstOrDefault(m => m.Id == matchUserToDelete.Id);
        Assert.True(deleted);
        Assert.Null(deletedMatchUserInDb);
    }

    [Fact]
    public void GetMatchUsersByMatchId_ShouldReturnMatchUsers()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetMatchUsersByMatchId_ShouldReturnMatchUsers));
        var matchUserService = MockMatchUserService(dbContext);
        var matchId = dbContext.RoundMatches.FirstOrDefault(x => x.MatchUser.Any()).Id;

        // Act
        var matchUsers = matchUserService.GetMatchUsersByMatchId(matchId);

        // Assert
        Assert.NotEmpty(matchUsers);
    }

    [Fact]
    public async Task GetMatchUsersById_ShouldReturnMatchUser()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetMatchUsersById_ShouldReturnMatchUser));
        var matchUserService = MockMatchUserService(dbContext);
        var matchUserId = dbContext.MatchUsers.FirstOrDefault().Id;

        // Act
        var matchUser = await matchUserService.GetMatchUsersById(matchUserId);

        // Assert
        Assert.NotNull(matchUser);
    }

    #region Helpers
    private IMatchUserService MockMatchUserService(GamexDbContext context)
    {
        return new MatchUserService(context);
    }
    #endregion
}
