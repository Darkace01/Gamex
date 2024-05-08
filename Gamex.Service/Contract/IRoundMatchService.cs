namespace Gamex.Service.Contract;

public interface IRoundMatchService
{
    Task CreateRoundMatch(RoundMatchCreateDTO roundMatch, CancellationToken cancellationToken = default);
    Task<(bool, string)> DeleteRoundMatch(Guid id, CancellationToken cancellationToken = default);
    IQueryable<RoundMatchDTO> GetAllRoundMatches();
    RoundMatchDTO? GetRoundMatchById(Guid id);
    Task<(bool, string)> UpdateRoundMatch(RoundMatchUpdateDTO roundMatch, CancellationToken cancellationToken = default);
}