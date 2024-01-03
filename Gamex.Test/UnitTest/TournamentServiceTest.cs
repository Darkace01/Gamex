using Gamex.DTO;
using Gamex.Service.Contract;
using Gamex.Service.Implementation;

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

    // write various test case, to test for different scenarios like invalid input, etc.
    // write test case for UpdateTournament, DeleteTournament, GetTournament, GetTournaments, etc.
    // write test case for other methods in TournamentService

    [Fact]
    public async Task UpdateTournament_ShouldUpdateTournament()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateTournament_ShouldUpdateTournament));
        var tournamentService = MockTournamentService(dbContext);
        var tournamentToUpdate = dbContext.Tournaments.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault();

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
        var testUser = dbContext.Users.FirstOrDefault();

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

    #region Helpers
    private ITournamentService MockTournamentService(GamexDbContext context)
    {
        return new TournamentService(context);
    }
    #endregion
}
