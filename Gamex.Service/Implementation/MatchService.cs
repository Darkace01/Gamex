namespace Gamex.Service.Implementation;
public class MatchService(GamexDbContext context) : IMatchService
{
    /// <summary>
    /// Retrieves all round matches.
    /// </summary>
    /// <returns>An <see cref="IQueryable{RoundMatchDTO}"/> representing the collection of round matches.</returns>
    public IQueryable<MatchDTO> GetAllMatches()
    {
        return context.RoundMatches
            .AsNoTracking()
            .Select(rm => new MatchDTO(rm.Id, rm.Name, rm.TournamentRoundId, new TournamentRoundDTO(rm.TournamentRound.Id, rm.TournamentRound.Name, rm.TournamentRound.Description)));
    }

    /// <summary>
    /// Retrieves a round match by its ID.
    /// </summary>
    /// <param name="id">The ID of the round match.</param>
    /// <returns>A <see cref="MatchDTO"/> representing the round match with the specified ID, or null if not found.</returns>
    public MatchDTO? GetMatchById(Guid id)
    {
        return context.RoundMatches
            .Include(rm => rm.TournamentRound)
            .AsNoTracking()
            .Select(rm => new MatchDTO(rm.Id, rm.Name, rm.TournamentRoundId, new TournamentRoundDTO(rm.TournamentRound.Id,
                                                                                                    rm.TournamentRound.Name,
                                                                                                    rm.TournamentRound.Description, new TournamentMiniDTO(rm.TournamentRound.Tournament.Id,
                                                                                                                                                          rm.TournamentRound.Tournament.Name,
                                                                                                                                                          rm.TournamentRound.Tournament.Description)),
                                                                                                    rm.MatchUser == null ? 0 : rm.MatchUser.Count))
            .FirstOrDefault(rm => rm.Id == id);
    }

    /// <summary>
    /// Retrieves all round matches by round ID.
    /// </summary>
    /// <param name="roundId">The ID of the round.</param>
    /// <returns>An <see cref="IQueryable{RoundMatchDTO}"/> representing the collection of round matches.</returns>
    public IQueryable<MatchDTO> GetAllMatchesByRoundId(Guid roundId)
    {
        return context.RoundMatches
            .Include(rm => rm.TournamentRound)
            .AsNoTracking()
            .Where(rm => rm.TournamentRoundId == roundId)
            .Select(rm => new MatchDTO(rm.Id, rm.Name, rm.TournamentRoundId, new TournamentRoundDTO(rm.TournamentRound.Id,
                                                                                                    rm.TournamentRound.Name,
                                                                                                    rm.TournamentRound.Description, new TournamentMiniDTO(rm.TournamentRound.Tournament.Id,
                                                                                                                                                          rm.TournamentRound.Tournament.Name,
                                                                                                                                                          rm.TournamentRound.Tournament.Description)),
                                                                                                    rm.MatchUser == null ? 0 : rm.MatchUser.Count));
    }

    /// <summary>
    /// Creates a new round match.
    /// </summary>
    /// <param name="roundMatch">The <see cref="MatchCreateDTO"/> containing the details of the round match to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the create was successful (true/false) and a message describing the result.</returns>
    public async Task<(bool, string)> CreateMatch(MatchCreateDTO roundMatch, CancellationToken cancellationToken = default)
    {
        var round = context.TournamentRounds.AsNoTracking().FirstOrDefault(r => r.Id == roundMatch.RoundId);
        if (round is null)
        {
            return (false, "Round not found");
        }
        var newRoundMatch = new RoundMatch
        {
            Name = roundMatch.Name,
            TournamentRoundId = roundMatch.RoundId
        };

        context.RoundMatches.Add(newRoundMatch);
        await context.SaveChangesAsync(cancellationToken);
        return (true, "Round match created successfully");
    }

    /// <summary>
    /// Updates an existing round match.
    /// </summary>
    /// <param name="roundMatch">The <see cref="MatchUpdateDTO"/> containing the updated details of the round match.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the update was successful (true/false) and a message describing the result.</returns>
    public async Task<(bool, string)> UpdateMatch(MatchUpdateDTO roundMatch, CancellationToken cancellationToken = default)
    {
        var existingRoundMatch = context.RoundMatches.FirstOrDefault(rm => rm.Id == roundMatch.Id);

        if (existingRoundMatch == null)
        {
            return (false, "Round match not found");
        }

        existingRoundMatch.Name = roundMatch.Name;

        await context.SaveChangesAsync(cancellationToken);
        return (true, "Round match updated successfully");
    }

    /// <summary>
    /// Deletes a round match by its ID.
    /// </summary>
    /// <param name="id">The ID of the round match to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the deletion was successful (true/false) and a message describing the result.</returns>
    public async Task<(bool, string)> DeleteMatch(Guid id, CancellationToken cancellationToken = default)
    {
        var existingRoundMatch = context.RoundMatches.FirstOrDefault(rm => rm.Id == id);

        if (existingRoundMatch == null)
        {
            return (false, "Round match not found");
        }

        context.RoundMatches.Remove(existingRoundMatch);
        await context.SaveChangesAsync(cancellationToken);
        return (true, "Round match deleted successfully");
    }
}
