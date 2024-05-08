namespace Gamex.Service.Contract;

public interface ITournamentRoundService
{
    Task CreateRound(TournamentRoundCreateDTO round);
    Task<(bool, string)> DeleteRound(Guid id);
    IQueryable<TournamentRoundDTO> GetAllRounds();
    TournamentRoundDTO? GetRoundById(Guid id);
    Task<(bool, string)> UpdateRound(TournamentRoundUpdateDTO round);
}