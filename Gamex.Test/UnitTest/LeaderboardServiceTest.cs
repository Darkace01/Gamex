﻿namespace Gamex.Test.UnitTest;

public class LeaderboardServiceTest : TestBase
{
    [Fact]
    public void GetLeaderboard_ShouldReturnLeaderboard()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetLeaderboard_ShouldReturnLeaderboard));
        var leaderboardService = MockLeaderboardService(dbContext);

        // Act
        var leaderboard = leaderboardService.GetLeaderboard();

        // Assert
        Assert.NotNull(leaderboard);
        //Assert.Equal(leaderboard.Count(), dbContext.UserTournaments.Count());
    }

    //GetLeaderboardWithTournamentFilter
    [Fact]
    public void GetLeaderboardWithTournamentFilter_ShouldReturnLeaderboard()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetLeaderboardWithTournamentFilter_ShouldReturnLeaderboard));
        var leaderboardService = MockLeaderboardService(dbContext);

        // Act
        var firstTournament = dbContext.Tournaments.First();
        var leaderboard = leaderboardService.GetLeaderboardWithTournamentFilter(firstTournament.Id);

        // Assert
        Assert.NotNull(leaderboard);
        //Assert.Equal(leaderboard.Count(), dbContext.UserTournaments.Count());
    }

    #region Helpers
    private LeaderboardService MockLeaderboardService(GamexDbContext dbContext)
    {
        return new LeaderboardService(dbContext);
    }
    #endregion
}

