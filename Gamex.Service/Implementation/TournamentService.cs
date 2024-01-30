namespace Gamex.Service.Implementation;

public class TournamentService(GamexDbContext context) : ITournamentService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Get tournament by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TournamentDTO> GetTournamentById(Guid id)
    {
        Tournament? tournament = await _context.Tournaments
            .AsNoTracking()
            .Include(t => t.Picture)
            .Include(t => t.UserTournaments)
            .ThenInclude(ut => ut.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tournament == null)
            return null;

        return new TournamentDTO
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Description = tournament.Description,
            IsFeatured = tournament.IsFeatured,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Time = tournament.Time,
            EntryFee = tournament.EntryFee,
            Rules = tournament.Rules,
            PictureId = tournament.Picture?.Id,
            PicturePublicId = tournament.Picture?.PublicId,
            PictureUrl = tournament.Picture?.FileUrl,

            TournamentUsers = tournament.UserTournaments.Select(ut => new TournamentUserDTO
            {
                UserId = ut.UserId,
                DisplayName = ut.User.DisplayName,
                PictureUrl = ut.User.Picture?.FileUrl,
            }).ToList()
        };
    }

    /// <summary>
    /// Get all tournaments
    /// </summary>
    /// <returns></returns>
    public IQueryable<TournamentDTO> GetAllTournaments()
    {
        var tournaments = _context.Tournaments
                             .AsNoTracking()
                             .Include(t => t.Picture)
                             .Include(t => t.UserTournaments)
                             .ThenInclude(ut => ut.User);

        return tournaments.Select(t => new TournamentDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsFeatured = t.IsFeatured,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Location = t.Location,
            Time = t.Time,
            EntryFee = t.EntryFee,
            Rules = t.Rules,
            PicturePublicId = t.Picture == null ? "" : t.Picture.PublicId,
            PictureUrl = t.Picture == null ? "" : t.Picture.FileUrl,

            TournamentUsers = t.UserTournaments.Select(ut => new TournamentUserDTO
            {
                UserId = ut.UserId,
                DisplayName = ut.User.DisplayName,
                PictureUrl = ut.User.Picture == null ? "" : ut.User.Picture.FileUrl,
            })
        });
    }

    /// <summary>
    /// Create a new tournament with the user
    /// </summary>
    /// <param name="tournament"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task CreateTournament(TournamentCreateDTO tournament, ApplicationUser user)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        await executionStrategy.Execute(
            async () =>
            {
                using var transaction = _context.Database.BeginTransaction();
                bool hasSaved = false;
                try
                {
                    Tournament newTournament = new()
                    {
                        Name = tournament.Name,
                        Description = tournament.Description,
                        IsFeatured = tournament.IsFeatured,
                        StartDate = tournament.StartDate,
                        EndDate = tournament.EndDate,
                        Location = tournament.Location,
                        Time = tournament.Time,
                        EntryFee = tournament.EntryFee,
                        Rules = tournament.Rules,
                        PictureId = tournament.PictureId,
                    };

                    await _context.Tournaments.AddAsync(newTournament);
                    await _context.SaveChangesAsync();
                    hasSaved = true;
                    UserTournament userTournament = new()
                    {
                        UserId = user.Id,
                        TournamentId = newTournament.Id,
                    };
                    await _context.UserTournaments.AddAsync(userTournament);
                    await _context.SaveChangesAsync();
                    hasSaved = true;
                    transaction.Commit();
                }
                catch (Exception)
                {
                    if (hasSaved)
                        transaction.Rollback();
                    throw;
                }
        });
    }
    /// <summary>
    /// Create a new tournament with the user without using SQL transaction for in-memory database
    /// </summary>
    /// <param name="tournament"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task CreateTournamentMock(TournamentCreateDTO tournament, ApplicationUser user)
    {
        //using var transaction = _context.Database.BeginTransaction();
        //bool hasSaved = false;
        try
        {
            Tournament newTournament = new()
            {
                Name = tournament.Name,
                Description = tournament.Description,
                IsFeatured = tournament.IsFeatured,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                Location = tournament.Location,
                Time = tournament.Time,
                EntryFee = tournament.EntryFee,
                Rules = tournament.Rules,
                PictureId = tournament.PictureId,
            };

            await _context.Tournaments.AddAsync(newTournament);
            await _context.SaveChangesAsync();
            //hasSaved = true;
            UserTournament userTournament = new()
            {
                UserId = user.Id,
                TournamentId = newTournament.Id,
            };
            await _context.UserTournaments.AddAsync(userTournament);
            await _context.SaveChangesAsync();
            //hasSaved = true;
            //transaction.Commit();
        }
        catch (Exception)
        {
            //if (hasSaved)
            //    transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Update a tournament with the user
    /// </summary>
    /// <param name="tournament"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user)
    {
        try
        {
            Tournament? existingTournament = await _context.Tournaments
                .Include(t => t.UserTournaments)
                .FirstOrDefaultAsync(t => t.Id == tournament.Id) ?? throw new Exception("Tournament not found");
            if (existingTournament.UserTournaments.All(ut => ut.UserId != user.Id))
                throw new Exception("You are not authorized to update this tournament");
            existingTournament.Name = tournament.Name;
            existingTournament.Description = tournament.Description;
            existingTournament.IsFeatured = tournament.IsFeatured;
            existingTournament.StartDate = tournament.StartDate;
            existingTournament.EndDate = tournament.EndDate;
            existingTournament.Location = tournament.Location;
            existingTournament.Time = tournament.Time;
            existingTournament.EntryFee = tournament.EntryFee;
            existingTournament.Rules = tournament.Rules;
            existingTournament.PictureId = tournament.PictureId;
            _context.Tournaments.Update(existingTournament);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Delete the tournament
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task DeleteTournament(Guid id, ApplicationUser user)
    {
        try
        {
            Tournament? existingTournament = await _context.Tournaments
                .Include(t => t.UserTournaments)
                .FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception("Tournament not found");
            if (existingTournament.UserTournaments.All(ut => ut.UserId != user.Id))
                throw new Exception("You are not authorized to delete this tournament");
            _context.Tournaments.Remove(existingTournament);
            await _context.SaveChangesAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }
}
