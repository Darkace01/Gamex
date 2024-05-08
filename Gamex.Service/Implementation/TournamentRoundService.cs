namespace Gamex.Service.Implementation;
public class TournamentRoundService(GamexDbContext context) : ITournamentRoundService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    ///  Retrieves all tournament rounds.
    ///   </summary>
    ///   <returns>An <see cref="IQueryable{TournamentRoundDTO}"/> representing the collection of tournament rounds.</returns>
    public IQueryable<TournamentRoundDTO> GetAllRounds()
    {
        return _context.TournamentRounds
            .Include(x => x.Tournament)
            .Include(x => x.Tournament)
            .AsNoTracking()
            .Select(r => new TournamentRoundDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Tournament = new TournamentMiniDTO(r.Tournament.Id, r.Tournament.Name, r.Tournament.Description)
            });
    }


    /// <summary>
    /// Retrieves a tournament round by its ID.
    /// </summary>
    /// <param name="id">The ID of the tournament round.</param>
    /// <returns>The tournament round with the specified ID, or null if not found.</returns>
    public TournamentRoundDTO? GetRoundById(Guid id)
    {
        return _context.TournamentRounds
            .Include(x => x.Tournament)
            .AsNoTracking()
            .Select(r => new TournamentRoundDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Tournament = new TournamentMiniDTO(r.Tournament.Id, r.Tournament.Name, r.Tournament.Description)
            })
            .FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Creates a new tournament round.
    /// </summary>
    /// <param name="round">The tournament round to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateRound(TournamentRoundCreateDTO round)
    {
        var newRound = new TournamentRound
        {
            Name = round.Name,
            Description = round.Description,
            TournamentId = round.TournamentId
        };

        _context.TournamentRounds.Add(newRound);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing tournament round.
    /// </summary>
    /// <param name="round">The updated tournament round.</param>
    /// <returns>A tuple indicating whether the update was successful (true/false) and an optional error message.</returns>
    public async Task<(bool, string)> UpdateRound(TournamentRoundUpdateDTO round)
    {
        var existingRound = await _context.TournamentRounds.FirstOrDefaultAsync(r => r.Id == round.Id);
        if (existingRound == null)
            return (false, "Round not found");

        existingRound.Name = round.Name;
        existingRound.Description = round.Description;
        existingRound.TournamentId = round.TournamentId;

        await _context.SaveChangesAsync();
        return (true, "");
    }

    /// <summary>
    /// Deletes a tournament round by its ID.
    /// </summary>
    /// <param name="id">The ID of the tournament round to delete.</param>
    /// <returns>A tuple indicating whether the deletion was successful (true/false) and an optional error message.</returns>
    public async Task<(bool, string)> DeleteRound(Guid id)
    {
        var existingRound = await _context.TournamentRounds.FirstOrDefaultAsync(r => r.Id == id);
        if (existingRound == null)
            return (false, "Round not found");

        _context.TournamentRounds.Remove(existingRound);
        await _context.SaveChangesAsync();
        return (true, "");
    }
}
