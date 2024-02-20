namespace Gamex.Service.Contract
{
    public interface ITournamentService
    {
        Task CreateTournament(TournamentCreateDTO tournament, ApplicationUser user);
        Task CreateTournamentMock(TournamentCreateDTO tournament, ApplicationUser user);
        Task DeleteTournament(Guid id, ApplicationUser user);
        IQueryable<TournamentDTO> GetAllTournaments();
        IQueryable<TournamentDTO> GetFeaturedTournaments();
        Task<TournamentDTO?> GetTournamentById(Guid id);
        Task<bool> JoinTournament(Guid id, ApplicationUser user);
        Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user, CancellationToken cancellationToken = default);
    }
}