namespace Gamex.Service.Contract;

public interface IMatchService
{
    Task<(bool, string)> CreateMatch(MatchCreateDTO roundMatch, CancellationToken cancellationToken = default);
    Task<(bool, string)> DeleteMatch(Guid id, CancellationToken cancellationToken = default);
    IQueryable<MatchDTO> GetAllMatches();
    IQueryable<MatchDTO> GetAllMatchesByRoundId(Guid roundId);
    MatchDTO? GetMatchById(Guid id);
    Task<(bool, string)> UpdateMatch(MatchUpdateDTO roundMatch, CancellationToken cancellationToken = default);
}