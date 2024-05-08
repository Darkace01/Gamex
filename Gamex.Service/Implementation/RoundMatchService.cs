namespace Gamex.Service.Implementation;
public class RoundMatchService(GamexDbContext context) : IRoundMatchService
{
    /// <summary>
    /// Retrieves all round matches.
    /// </summary>
    /// <returns>An <see cref="IQueryable{RoundMatchDTO}"/> representing the collection of round matches.</returns>
    public IQueryable<RoundMatchDTO> GetAllRoundMatches()
    {
        return context.RoundMatches
            .AsNoTracking()
            .Select(rm => new RoundMatchDTO(rm.Id, rm.Name, rm.TournamentRoundId));
    }

    /// <summary>
    /// Retrieves a round match by its ID.
    /// </summary>
    /// <param name="id">The ID of the round match.</param>
    /// <returns>A <see cref="RoundMatchDTO"/> representing the round match with the specified ID, or null if not found.</returns>
    public RoundMatchDTO? GetRoundMatchById(Guid id)
    {
        return context.RoundMatches
            .Include(rm => rm.TournamentRound)
            .AsNoTracking()
            .Select(rm => new RoundMatchDTO(rm.Id, rm.Name, rm.TournamentRoundId, new TournamentRoundDTO(rm.TournamentRound.Id, rm.TournamentRound.Name, rm.TournamentRound.Description)))
            .FirstOrDefault(rm => rm.Id == id);
    }

    /// <summary>
    /// Creates a new round match.
    /// </summary>
    /// <param name="roundMatch">The <see cref="RoundMatchCreateDTO"/> containing the details of the round match to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CreateRoundMatch(RoundMatchCreateDTO roundMatch, CancellationToken cancellationToken = default)
    {
        var newRoundMatch = new RoundMatch
        {
            Name = roundMatch.Name,
            TournamentRoundId = roundMatch.RoundId
        };

        context.RoundMatches.Add(newRoundMatch);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing round match.
    /// </summary>
    /// <param name="roundMatch">The <see cref="RoundMatchUpdateDTO"/> containing the updated details of the round match.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple indicating whether the update was successful (true/false) and a message describing the result.</returns>
    public async Task<(bool, string)> UpdateRoundMatch(RoundMatchUpdateDTO roundMatch, CancellationToken cancellationToken = default)
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
    public async Task<(bool, string)> DeleteRoundMatch(Guid id, CancellationToken cancellationToken = default)
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
