using Gamex.Common;
using Gamex.DTO;
using Gamex.Models;
using System.Threading;

namespace Gamex.Service.Implementation;

public class TournamentService(GamexDbContext context) : ITournamentService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Get tournament by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TournamentDTO?> GetTournamentById(Guid id)
    {
        var tournament = await _context.Tournaments
            .AsNoTracking()
            .Include(t => t.Picture)
            .Include(t => t.CoverPicture)
            .Include(t => t.Categories)
            .Include(t => t.UserTournaments)
                .ThenInclude(ut => ut.User.Picture)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tournament == null)
        {
            return null;
        }

        var tournamentDTO = new TournamentDTO
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
            PicturePublicId = tournament.Picture?.PublicId ?? "",
            PictureUrl = tournament.Picture?.FileUrl ?? "",
            CoverPicturePublicId = tournament.CoverPicture?.PublicId ?? "",
            CoverPictureUrl = tournament.CoverPicture?.FileUrl ?? "",
            AvailableSlot = tournament.AvailableSlot ?? 0,
            Categories = tournament.Categories.Select(tc => new TournamentCategoryDTO
            {
                Id = tc.Id,
                Name = tc.Name
            }),
            TournamentUsers = tournament.UserTournaments
                .OrderBy(x => x.Point)
                .Select((ut, index) => new TournamentUserDTO
                {
                    UserId = ut.UserId,
                    Email = ut.User.Email,
                    CreatorId = ut.CreatorId,
                    DisplayName = ut.User.DisplayName,
                    PictureUrl = ut.User.Picture?.FileUrl ?? "",
                    Points = ut.Point ?? 0,
                    Rank = index + 1
                })
        };

        return tournamentDTO;
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
            .Include(t => t.CoverPicture)
            .Include(t => t.Categories)
            .Include(t => t.UserTournaments)
            .ThenInclude(ut => ut.User)
            .Include(t => t.UserTournaments)
            .ThenInclude(ut => ut.User.Picture);

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
            CoverPicturePublicId = t.CoverPicture == null ? "" : t.CoverPicture.PublicId,
            CoverPictureUrl = t.CoverPicture == null ? "" : t.CoverPicture.FileUrl,
            AvailableSlot = t.AvailableSlot ?? 0,
            Categories = t.Categories.Select(tc => new TournamentCategoryDTO
            {
                Id = tc.Id,
                Name = tc.Name
            }),

            //TournamentUsers = t.UserTournaments.Select(ut => new TournamentUserDTO
            //{
            //    UserId = ut.UserId,
            //    CreatorId = ut.CreatorId,
            //    DisplayName = ut.User.DisplayName,
            //    PictureUrl = ut.User.Picture == null ? "" : ut.User.Picture.FileUrl,
            //    Points = ut.Point ?? 0
            //})
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
                        CoverPictureId = tournament.CoverPictureId,
                        Categories = _context.TournamentCategories.Where(tc => tournament.CategoryIds.Contains(tc.Id)).ToList(),
                    };

                    await _context.Tournaments.AddAsync(newTournament);
                    await _context.SaveChangesAsync();
                    hasSaved = true;
                    UserTournament userTournament = new()
                    {
                        UserId = user.Id,
                        CreatorId = user.Id,
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
                CoverPictureId = tournament.CoverPictureId,
            };

            if (tournament.CategoryIds != null)
                newTournament.Categories = _context.TournamentCategories.Where(tc => tournament.CategoryIds.Contains(tc.Id)).ToList();

            await _context.Tournaments.AddAsync(newTournament);
            await _context.SaveChangesAsync();
            //hasSaved = true;
            UserTournament userTournament = new()
            {
                UserId = user.Id,
                CreatorId = user.Id,
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
    public async Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            Tournament? existingTournament = await _context.Tournaments
                .Include(t => t.UserTournaments)
                .Include(t => t.Categories)
                .Include(t => t.Picture)
                .Include(t => t.CoverPicture)
                .FirstOrDefaultAsync(t => t.Id == tournament.Id, cancellationToken) ?? throw new Exception("Tournament not found");
            var adminRoleId = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Name == AppConstant.AdminUserRole)?.Id;
            List<string> adminUsers = new List<string>();
            if (!string.IsNullOrWhiteSpace(adminRoleId))
            {
                adminUsers = await _context.UserRoles.AsNoTracking().Where(ur => ur.RoleId == adminRoleId).Select(ur => ur.UserId).ToListAsync(cancellationToken);
            }

            bool isAuthorized = existingTournament.UserTournaments.Any(ut => ut.CreatorId != user.Id || adminUsers.Any(x => x == user.Id));
            if (!isAuthorized)
            {
                throw new Exception("You are not authorized to update this tournament");
            }

            existingTournament.Name = tournament.Name;
            existingTournament.Description = tournament.Description;
            existingTournament.IsFeatured = tournament.IsFeatured;
            existingTournament.StartDate = tournament.StartDate;
            existingTournament.EndDate = tournament.EndDate;
            existingTournament.Location = tournament.Location;
            existingTournament.Time = tournament.Time;
            existingTournament.EntryFee = tournament.EntryFee;
            existingTournament.Rules = tournament.Rules;

            if (tournament.PictureId.HasValue && existingTournament.PictureId != tournament.PictureId)
            {
                existingTournament.PictureId = tournament.PictureId;
            }

            if (tournament.CoverPictureId.HasValue && existingTournament.CoverPictureId != tournament.CoverPictureId)
            {
                existingTournament.CoverPictureId = tournament.CoverPictureId;
            }

            // if tournament.Categories is not null, delete he existing categories and add the new ones
            if (tournament.CategoryIds != null)
            {
                existingTournament.Categories.Clear();
                existingTournament.Categories = _context.TournamentCategories.Where(tc => tournament.CategoryIds.Contains(tc.Id)).ToList();
            }

            await _context.SaveChangesAsync(cancellationToken);
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
            var adminRoleId = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Name == AppConstant.AdminUserRole)?.Id;
            List<string> adminUsers = [];
            if (!string.IsNullOrWhiteSpace(adminRoleId))
            {
                adminUsers = _context.UserRoles.AsNoTracking().Where(ur => ur.RoleId == adminRoleId).Select(ur => ur.UserId).ToList();
            }
            if (!existingTournament.UserTournaments.Any(ut => ut.CreatorId != user.Id || adminUsers.Any(x => x == user.Id)))
                throw new Exception("You are not authorized to delete this tournament");
            _context.Tournaments.Remove(existingTournament);
            await _context.SaveChangesAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// Join a tournament mock for unit testing without a transaction
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> JoinTournamentMock(Guid id, ApplicationUser user)
    {
        try
        {
            Tournament? existingTournament = await _context.Tournaments
                .Include(t => t.UserTournaments)
                .FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception("Tournament not found");

            if (existingTournament.AvailableSlot <= existingTournament.UserTournaments.Count)
            {
                throw new Exception("Tournament is full");
            }
            if (existingTournament.EntryFee > 0)
            {
                var paymentTransaction = new PaymentTransaction
                {
                    UserId = user.Id,
                    TournamentId = id,
                    Amount = -existingTournament.EntryFee,
                    Status = Models.TransactionStatus.Success,
                    TransactionReference = CommonHelpers.GenerateRandomString(10)
                };

                _context.PaymentTransactions.Add(paymentTransaction);
                await _context.SaveChangesAsync();
            }
            UserTournament userTournament = new()
            {
                UserId = user.Id,
                TournamentId = existingTournament.Id,
            };
            await _context.UserTournaments.AddAsync(userTournament);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// Join a tournament
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns true if the user successfully joins the tournament, otherwise false</returns>
    public async Task<bool> JoinTournament(Guid id, ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
        {
            using var transaction = _context.Database.BeginTransaction();
            bool hasSaved = false;
            try
            {
                Tournament? existingTournament = await _context.Tournaments
                    .Include(t => t.UserTournaments)
                    .FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception("Tournament not found");

                if (existingTournament.UserTournaments.Any(ut => ut.UserId == user.Id))
                {
                    return true;
                }

                if(existingTournament.AvailableSlot < existingTournament.UserTournaments.Count)
                {
                    throw new Exception("Tournament is full");
                }

                //debit the user if the tournament has an entry fee
                if (existingTournament.EntryFee > 0)
                {
                    var paymentTransaction = new PaymentTransaction
                    {
                        UserId = user.Id,
                        TournamentId = id,
                        Amount = -existingTournament.EntryFee,
                        Status = Models.TransactionStatus.Success,
                        TransactionReference = CommonHelpers.GenerateRandomString(10)
                    };

                    _context.PaymentTransactions.Add(paymentTransaction);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                UserTournament userTournament = new()
                {
                    UserId = user.Id,
                    TournamentId = existingTournament.Id,
                };

                await _context.UserTournaments.AddAsync(userTournament);
                await _context.SaveChangesAsync(cancellationToken);
                hasSaved = true;
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                if (hasSaved)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                throw;
            }
        });
    }

    /// <summary>
    /// Joins a tournament with a transaction reference
    /// </summary>
    /// <param name="id">The ID of the tournament</param>
    /// <param name="user">The user joining the tournament</param>
    /// <param name="transactionReference">The transaction reference</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Returns true if the user successfully joins the tournament, otherwise false</returns>
    public async Task<bool> JoinTournamentWithTransactionReference(Guid id, ApplicationUser user, string transactionReference = "", CancellationToken cancellationToken = default)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                Tournament? existingTournament = await _context.Tournaments
                    .Include(t => t.UserTournaments)
                    .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

                if (existingTournament == null)
                {
                    throw new Exception("Tournament not found");
                }

                if (existingTournament.UserTournaments.Any(ut => ut.UserId == user.Id))
                {
                    return true;
                }

                if (existingTournament.AvailableSlot < existingTournament.UserTournaments.Count)
                {
                    throw new Exception("Tournament is full");
                }
                Guid? creditId = null;
                if (!string.IsNullOrWhiteSpace(transactionReference))
                {
                    var creditPaymentTransaction = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.TransactionReference == transactionReference && x.Status == Models.TransactionStatus.Pending, cancellationToken);
                    if (creditPaymentTransaction != null)
                    {
                        creditPaymentTransaction.Status = Models.TransactionStatus.Success;
                        creditPaymentTransaction.DateModified = DateTime.Now;
                        creditId = creditPaymentTransaction.Id;
                    }
                }

                //debit the user if the tournament has an entry fee
                if (existingTournament.EntryFee > 0)
                {
                    var debitPaymentTransaction = new PaymentTransaction
                    {
                        UserId = user.Id,
                        TournamentId = id,
                        Amount = -existingTournament.EntryFee,
                        Status = Models.TransactionStatus.Success,
                        TransactionReference = CommonHelpers.GenerateRandomString(10)
                    };

                    _context.PaymentTransactions.Add(debitPaymentTransaction);
                }

                UserTournament userTournament = new()
                {
                    UserId = user.Id,
                    TournamentId = existingTournament.Id,
                    PaymentTransactionId = creditId,
                    DateJoined = DateTime.Now
                };

                _context.UserTournaments.Add(userTournament);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
    /// <summary>
    /// Gets the featured tournaments
    /// </summary>
    /// <returns></returns>
    public IQueryable<TournamentDTO> GetFeaturedTournaments()
    {
        return GetAllTournaments().Where(t => t.IsFeatured);
    }
}
