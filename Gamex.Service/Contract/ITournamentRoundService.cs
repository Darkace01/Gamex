namespace Gamex.Service.Contract;

public interface ITournamentRoundService
{
    Task CreateRound(TournamentRoundCreateDTO round, CancellationToken cancellationToken = default);
    Task<(bool, string)> DeleteRound(Guid id, CancellationToken cancellationToken = default);
    IQueryable<TournamentRoundDTO> GetAllRounds();
    TournamentRoundDTO? GetRoundById(Guid id);
    Task<(bool, string)> UpdateRound(TournamentRoundUpdateDTO round, CancellationToken cancellationToken = default);
}