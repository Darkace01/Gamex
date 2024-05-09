namespace Gamex.Service.Contract;

public interface IRoundService
{
    Task CreateRound(TournamentRoundCreateDTO round, CancellationToken cancellationToken = default);
    Task<(bool, string)> DeleteRound(Guid id, CancellationToken cancellationToken = default);
    IQueryable<TournamentRoundDTO> GetAllRounds();
    IQueryable<TournamentRoundDTO> GetAllRoundsByTournamentId(Guid tournamentId);
    TournamentRoundDTO? GetRoundById(Guid id);
    Task<(bool, string)> UpdateRound(TournamentRoundUpdateDTO round, CancellationToken cancellationToken = default);
}