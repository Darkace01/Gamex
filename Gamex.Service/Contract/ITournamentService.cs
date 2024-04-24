namespace Gamex.Service.Contract
{
    public interface ITournamentService
    {
        /// <summary>
        /// Creates a new tournament.
        /// </summary>
        /// <param name="tournament">The tournament details.</param>
        /// <param name="user">The user creating the tournament.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateTournament(TournamentCreateDTO tournament, ApplicationUser user);

        /// <summary>
        /// Creates a new tournament for testing purposes.
        /// </summary>
        /// <param name="tournament">The tournament details.</param>
        /// <param name="user">The user creating the tournament.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateTournamentMock(TournamentCreateDTO tournament, ApplicationUser user);

        /// <summary>
        /// Deletes a tournament.
        /// </summary>
        /// <param name="id">The ID of the tournament to delete.</param>
        /// <param name="user">The user deleting the tournament.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteTournament(Guid id, ApplicationUser user);

        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>An IQueryable of TournamentDTO representing all tournaments.</returns>
        IQueryable<TournamentDTO> GetAllTournaments();

        /// <summary>
        /// Gets featured tournaments.
        /// </summary>
        /// <returns>An IQueryable of TournamentDTO representing featured tournaments.</returns>
        IQueryable<TournamentDTO> GetFeaturedTournaments();

        /// <summary>
        /// Gets a tournament by ID.
        /// </summary>
        /// <param name="id">The ID of the tournament to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The result is the TournamentDTO if found, otherwise null.</returns>
        Task<TournamentDTO?> GetTournamentById(Guid id);

        /// <summary>
        /// Gets the tournament users by tournament id
        /// </summary>
        /// <param name="id">The id of the tournament</param>
        /// <returns>The tournament users</returns>
        IQueryable<TournamentUserDTO> GetTournamentUsers(Guid id);

        /// <summary>
        /// Joins a tournament.
        /// </summary>
        /// <param name="id">The ID of the tournament to join.</param>
        /// <param name="user">The user joining the tournament.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result is true if the user successfully joins the tournament, otherwise false.</returns>
        Task<(bool, string)> JoinTournament(Guid id, ApplicationUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Joins a tournament for testing purposes.
        /// </summary>
        /// <param name="id">The ID of the tournament to join.</param>
        /// <param name="user">The user joining the tournament.</param>
        /// <returns>A task representing the asynchronous operation. The result is true if the user successfully joins the tournament, otherwise false.</returns>
        Task<(bool, string)> JoinTournamentMock(Guid id, ApplicationUser user);
        /// <summary>
        /// Joins a tournament with a transaction reference
        /// </summary>
        /// <param name="id">The ID of the tournament</param>
        /// <param name="user">The user joining the tournament</param>
        /// <param name="transactionReference">The transaction reference</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Returns a tuple with a boolean indicating if the user successfully joins the tournament and a string message</returns>
        Task<(bool, string)> JoinTournamentWithTransactionReference(Guid id, ApplicationUser user, string transactionReference = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a tournament.
        /// </summary>
        /// <param name="tournament">The updated tournament details.</param>
        /// <param name="user">The user updating the tournament.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the waitlist status of a user in a tournament.
        /// </summary>
        /// <param name="id">The ID of the tournament.</param>
        /// <param name="user">The user whose waitlist status needs to be updated.</param>
        /// <param name="passedWaitList">The new waitlist status of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserWaitListStatus(Guid id, string userId, bool passedWaitList, CancellationToken cancellationToken = default);
    }
}