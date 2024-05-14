namespace Gamex.Test.UnitTest;

public class TournamentServiceTest : TestBase
{
    [Fact]
    public async Task CreateTournament_ShouldCreateTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateTournament_ShouldCreateTournament));
        var tournamentService = MockTournamentService(dbContext);

        // Act
        TournamentCreateDTO tournament = new()
        {
            Name = "Test Tournament",
            Description = "Test Description",
            Location = "Test Location",
            EntryFee = 100,
            Rules = "Test Rules",
            StartDateString = "01/01/2024",
            EndDateString = "01/01/2024",
            TimeString = "01:00",
        };

        var testUser = dbContext.Users.FirstOrDefault();
        await tournamentService.CreateTournamentMock(tournament, testUser);

        // Assert
        var tournamentInDb = dbContext.Tournaments.FirstOrDefault();
        Assert.NotNull(tournamentInDb);
        var userTournamentInDb = dbContext.UserTournaments.FirstOrDefault();
        Assert.NotNull(userTournamentInDb);
    }


    [Fact]
    public async Task UpdateTournament_ShouldUpdateTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateTournament_ShouldUpdateTournament));
        var tournamentService = MockTournamentService(dbContext);
        var tournamentToUpdate = dbContext.Tournaments.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault();
        var tournamnetCategories = dbContext.TournamentCategories.ToList();
        TournamentUpdateDTO updatedTournament = new()
        {
            Name = "Updated Tournament",
            Description = "Updated Description",
            Location = "Updated Location",
            EntryFee = 200,
            Rules = "Updated Rules",
            StartDateString = "02/02/2024",
            EndDateString = "02/02/2024",
            TimeString = "02:00",
            Id = tournamentToUpdate.Id,
            CategoryIds = tournamnetCategories.Select(x => x.Id).ToList()
        };

        // Act
        await tournamentService.UpdateTournament(updatedTournament, testUser);

        // Assert
        var updatedTournamentInDb = dbContext.Tournaments.FirstOrDefault();
        Assert.Equal("Updated Tournament", updatedTournamentInDb.Name);
    }

    [Fact]
    public async Task DeleteTournament_ShouldDeleteTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteTournament_ShouldDeleteTournament));
        var tournamentService = MockTournamentService(dbContext);
        var tournamentToDelete = dbContext.Tournaments.FirstOrDefault();
        var testUser = tournamentToDelete?.UserTournaments.FirstOrDefault()?.User;

        // Act
        await tournamentService.DeleteTournament(tournamentToDelete.Id, testUser);

        // Assert
        var deletedTournamentInDb = dbContext.Tournaments.FirstOrDefault(t => t.Id == tournamentToDelete.Id);
        Assert.Null(deletedTournamentInDb);
    }

    [Fact]
    public async Task GetTournamentById_ShouldReturnTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTournamentById_ShouldReturnTournament));
        var tournamentService = MockTournamentService(dbContext);
        var tournamentToGet = dbContext.Tournaments.FirstOrDefault();

        // Act
        var tournament = await tournamentService.GetTournamentById(tournamentToGet.Id);

        // Assert
        Assert.NotNull(tournament);
        Assert.Equal(tournamentToGet.Id, tournament.Id);
    }

    [Fact]
    public void GetAllTournaments_ShouldReturnAllTournaments()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllTournaments_ShouldReturnAllTournaments));
        var tournamentService = MockTournamentService(dbContext);

        // Act
        var tournaments = tournamentService.GetAllTournaments();

        // Assert
        Assert.NotNull(tournaments);
        Assert.Equal(dbContext.Tournaments.Count(), tournaments.Count());
    }

    [Fact]
    public async Task JoinTournament_ShouldJoinTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(JoinTournament_ShouldJoinTournament));
        var tournamentService = MockTournamentService(dbContext);
        var tournamentToJoin = dbContext.Tournaments.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault(x => x.Email == "user2@email.com");

        // Act
        await tournamentService.JoinTournamentMock(tournamentToJoin.Id, testUser);

        // Assert
        var joinedTournamentInDb = dbContext.UserTournaments.FirstOrDefault(t => t.TournamentId == tournamentToJoin.Id && t.UserId == testUser.Id);
        Assert.NotNull(joinedTournamentInDb);
    }

    [Fact]
    public void GetFeaturedTournaments_ShouldReturnFeaturedTournaments()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetFeaturedTournaments_ShouldReturnFeaturedTournaments));
        var tournamentService = MockTournamentService(dbContext);

        // Act
        var tournaments = tournamentService.GetFeaturedTournaments();

        // Assert
        Assert.NotNull(tournaments);
        Assert.Equal(2, tournaments.Count());
    }

    [Fact]
    public async Task GetTournamentUserDetail_ShouldReturnTournamentUserDetail()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTournamentUserDetail_ShouldReturnTournamentUserDetail));
        var tournamentService = MockTournamentService(dbContext);
        var tournament = dbContext.Tournaments.FirstOrDefault();
        var user = dbContext.Users.FirstOrDefault();

        // Act
        var tournamentUserDetail = await tournamentService.GetTournamentUserDetail(tournament.Id, user.Id);

        // Assert
        Assert.NotNull(tournamentUserDetail);
        Assert.Equal(user.Id, tournamentUserDetail.UserId);
    }

    [Fact]
    public void GetTournamentUsers_ShouldReturnTournamentUsers()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTournamentUsers_ShouldReturnTournamentUsers));
        var tournamentService = MockTournamentService(dbContext);
        var tournament = dbContext.Tournaments.FirstOrDefault();

        // Act
        var tournamentUsers = tournamentService.GetTournamentUsers(tournament.Id);

        // Assert
        Assert.NotNull(tournamentUsers);
        Assert.Equal(1, tournamentUsers.Count());
    }

    [Fact]
    public async Task UpdateUserTournamentDetails_ShouldUpdateUserTournamentDetails()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateUserTournamentDetails_ShouldUpdateUserTournamentDetails));
        var tournamentService = MockTournamentService(dbContext);
        var tournament = dbContext.Tournaments.FirstOrDefault();
        var user = dbContext.Users.FirstOrDefault();
        var tournamentUser = dbContext.UserTournaments.FirstOrDefault();

        TournamentUserUpdateDTO updatedTournamentUser = new()
        {
            DisplayName = "Updated Display Name",
            Email = "update@email.com",
            TournamentName = "Updated Tournament Name",
            TournamentId = tournament.Id,
            UserId = user.Id,
            Points = 100,
            IsInWaitList = false,
            Win = true,
            Loss = false,
            Draw = false
        };

        await tournamentService.UpdateUserTournamentDetails(updatedTournamentUser);

        // Assert
        var updatedTournamentUserInDb = dbContext.UserTournaments.FirstOrDefault(x => x.TournamentId == tournament.Id);
        Assert.Equal(100, updatedTournamentUserInDb.Point);
    }

    [Fact]
    public async Task UpdateUserWaitListStatus_ShouldUpdateUserWaitListStatus()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateUserWaitListStatus_ShouldUpdateUserWaitListStatus));
        var tournamentService = MockTournamentService(dbContext);
        var tournament = dbContext.Tournaments.FirstOrDefault();
        var user = dbContext.Users.FirstOrDefault();

        // Act
        await tournamentService.UpdateUserWaitListStatus(tournament.Id, user.Id, false);

        // Assert
        var updatedTournamentUserInDb = dbContext.UserTournaments.FirstOrDefault(x => x.TournamentId == tournament.Id);
        Assert.False(updatedTournamentUserInDb.WaitList);
    }

    //[Fact]
    //public async Task JoinTournamentWithTransactionReference_ShouldJoinTournamentWithTransactionReference()
    //{
    //    // Arrange
    //    var dbContext = GetSampleData(nameof(JoinTournamentWithTransactionReference_ShouldJoinTournamentWithTransactionReference));
    //    var tournamentService = MockTournamentService(dbContext);
    //    var tournament = dbContext.Tournaments.FirstOrDefault();
    //    var user = dbContext.Users.FirstOrDefault();

    //    // Act
    //    await tournamentService.JoinTournamentWithTransactionReference(tournament.Id, user);

    //    // Assert
    //    var joinedTournamentInDb = dbContext.UserTournaments.FirstOrDefault(x => x.TournamentId == tournament.Id);
    //    Assert.NotNull(joinedTournamentInDb);
    //}
    #region Helpers
    private ITournamentService MockTournamentService(GamexDbContext context)
    {
        return new TournamentService(context);
    }
    #endregion
}
