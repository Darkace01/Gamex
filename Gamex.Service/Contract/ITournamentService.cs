namespace Gamex.Service.Contract
{
    public interface ITournamentService
    {
        Task CreateTournament(TournamentCreateDTO tournament, ApplicationUser user);
        Task DeleteTournament(Guid id, ApplicationUser user);
        IQueryable<TournamentDTO> GetAllTournaments();
        Task<TournamentDTO> GetTournamentById(Guid id);
        Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user);
    }
}