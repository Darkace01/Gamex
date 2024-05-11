namespace Gamex.Service.Contract;

public interface IMatchUserService
{
    Task<(bool, string)> CreateMatchUser(MatchUserCreateDTO matchUserCreateDTO, CancellationToken cancellationToken = default);
    Task<(bool, string)> DeleteMatchUser(Guid id, CancellationToken cancellationToken = default);
    Task<MatchUserDTO?> GetMatchUsersById(Guid matchId, CancellationToken cancellationToken = default);
    IQueryable<MatchUserDTO> GetMatchUsersByMatchId(Guid matchId);
    Task<(bool, string)> UpdateMatchUser(MatchUserUpdateDTO matchUserUpdateDTO, CancellationToken cancellationToken = default);
}