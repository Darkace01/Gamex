﻿using Gamex.Common;

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
        return await GetAllTournaments().FirstOrDefaultAsync(t => t.Id == id);
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

            Categories = t.Categories.Select(tc => new TournamentCategoryDTO
            {
                Id = tc.Id,
                Name = tc.Name
            }),

            TournamentUsers = t.UserTournaments.Select(ut => new TournamentUserDTO
            {
                UserId = ut.UserId,
                CreatorId = ut.CreatorId,
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
                .FirstOrDefaultAsync(t => t.Id == tournament.Id,cancellationToken);

            if (existingTournament is null)
            {
                throw new Exception("Tournament not found");
            }

            var adminRoleId = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Name == AppConstant.AdminUserRole)?.Id;
            List<string> adminUsers = [];
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

            if (tournament.CategoryIds != null)
            {
                var categoryList = _context.TournamentCategories.AsNoTracking().Where(tc => tournament.CategoryIds.Contains(tc.Id)).ToList();
                if (categoryList.Count > 0)
                {
                    existingTournament.Categories.Clear();
                    existingTournament.Categories?.AddRange(categoryList);
                }
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

    public async Task<bool> JoinTournament(Guid id, ApplicationUser user)
    {
        try
        {
            Tournament? existingTournament = await _context.Tournaments
                .Include(t => t.UserTournaments)
                .FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception("Tournament not found");
            //if (existingTournament.UserTournaments.Any(ut => ut.UserId == user.Id))
            //    throw new Exception("You have already joined this tournament");
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

    public IQueryable<TournamentDTO> GetFeaturedTournaments()
    {
        return GetAllTournaments().Where(t => t.IsFeatured);
    }
}
