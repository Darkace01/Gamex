namespace Gamex.Service.Implementation;
public class MatchUserService(GamexDbContext context) : IMatchUserService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Get the match user by match ID.
    /// </summary>
    /// <param name="matchId">The ID of the match.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The match user DTO.</returns>
    public async Task<MatchUserDTO?> GetMatchUsersById(Guid matchId, CancellationToken cancellationToken = default)
    {
        return await _context.MatchUsers
            .Include(x => x.User)
            .AsNoTracking()
            .Where(mu => mu.Id == matchId)
            .Select(mu => new MatchUserDTO(mu.Id,
                                           mu.UserId,
                                           mu.User.Email,
                                           mu.User.DisplayName,
                                           mu.User.Picture == null ? "" : mu.User.Picture.FileUrl,
                                           mu.Point ?? 0,
                                           0,
                                           mu.Win ?? false,
                                           mu.Loss ?? false,
                                           mu.Draw ?? false,
                                           mu.MatchId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get the match users by match ID.
    /// </summary>
    /// <param name="matchId">The ID of the match.</param>
    /// <returns>The match user DTO queryable.</returns>
    public IQueryable<MatchUserDTO> GetMatchUsersByMatchId(Guid matchId)
    {
        return _context.MatchUsers
            .Include(x => x.User)
            .AsNoTracking()
            .Where(mu => mu.MatchId == matchId)
            .Select(mu => new MatchUserDTO(mu.Id,
                                        mu.UserId,
                                           mu.User.Email,
                                           mu.User.DisplayName,
                                           //mu.User.Picture == null ? "" : mu.User.Picture.FileUrl,
                                           "",
                                           mu.Point ?? 0,
                                           0,
                                           mu.Win ?? false,
                                           mu.Loss ?? false,
                                           mu.Draw ?? false,
                                           mu.MatchId));
    }

    /// <summary>
    /// Create a match user.
    /// </summary>
    /// <param name="matchUserCreateDTO">The match user create DTO.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the match user was created successfully and the result message.</returns>
    public async Task<(bool, string)> CreateMatchUser(MatchUserCreateDTO matchUserCreateDTO, CancellationToken cancellationToken = default)
    {
        // check if userId existing in the userTournament table for that particular tournament 
        var canJoinMatch = await _context.Tournaments
            .Join(_context.TournamentRounds, t => t.Id, r => r.TournamentId, (t, r) => new { Tournament = t, Round = r })
            .Join(_context.UserTournaments, tr => tr.Tournament.Id, tu => tu.TournamentId, (tr, tu) => new { TournamentRound = tr, UserTournament = tu })
            .Where(x => x.UserTournament.UserId == matchUserCreateDTO.UserId && x.UserTournament.WaitList == true)
            .Select(x => x.TournamentRound.Tournament.Id)
            .AnyAsync(cancellationToken);

        if (!canJoinMatch)
        {
            return (false, "User not in tournament");
        }

        // check if user already in match
        var userInMatch = await _context.MatchUsers
            .AsNoTracking()
            .AnyAsync(mu => mu.UserId == matchUserCreateDTO.UserId && mu.MatchId == matchUserCreateDTO.MatchId, cancellationToken);
        if (userInMatch)
        {
            return (false, "User already in match");
        }

        var matchUser = new MatchUser
        {
            UserId = matchUserCreateDTO.UserId,
            Point = matchUserCreateDTO.Points,
            Win = matchUserCreateDTO.Win,
            Loss = matchUserCreateDTO.Loss,
            Draw = matchUserCreateDTO.Draw,
            MatchId = matchUserCreateDTO.MatchId
        };
        _context.MatchUsers.Add(matchUser);
        await _context.SaveChangesAsync(cancellationToken);
        return (true, "Match user created");
    }

    /// <summary>
    /// Update a match user.
    /// </summary>
    /// <param name="matchUserUpdateDTO">The match user update DTO.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the match user was updated successfully and the result message.</returns>
    public async Task<(bool, string)> UpdateMatchUser(MatchUserUpdateDTO matchUserUpdateDTO, CancellationToken cancellationToken = default)
    {
        var existingMatchUser = await _context.MatchUsers.FirstOrDefaultAsync(mu => mu.Id == matchUserUpdateDTO.Id, cancellationToken);
        if (existingMatchUser == null)
        {
            return (false, "Match user not found");
        }

        existingMatchUser.Point = matchUserUpdateDTO.Points;
        existingMatchUser.Win = matchUserUpdateDTO.Win;
        existingMatchUser.Loss = matchUserUpdateDTO.Loss;
        existingMatchUser.Draw = matchUserUpdateDTO.Draw;

        await _context.SaveChangesAsync(cancellationToken);
        return (true, "Match user updated");
    }

    /// <summary>
    /// Delete a match user by ID.
    /// </summary>
    /// <param name="id">The ID of the match user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the match user was deleted successfully and the result message.</returns>
    public async Task<(bool, string)> DeleteMatchUser(Guid id, CancellationToken cancellationToken = default)
    {
        var existingMatchUser = await _context.MatchUsers.FirstOrDefaultAsync(mu => mu.Id == id, cancellationToken);
        if (existingMatchUser == null)
        {
            return (false, "Match user not found");
        }

        _context.MatchUsers.Remove(existingMatchUser);
        await _context.SaveChangesAsync(cancellationToken);
        return (true, "Match user deleted");
    }
}
