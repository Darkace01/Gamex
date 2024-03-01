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
        /// Joins a tournament.
        /// </summary>
        /// <param name="id">The ID of the tournament to join.</param>
        /// <param name="user">The user joining the tournament.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result is true if the user successfully joins the tournament, otherwise false.</returns>
        Task<bool> JoinTournament(Guid id, ApplicationUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Joins a tournament for testing purposes.
        /// </summary>
        /// <param name="id">The ID of the tournament to join.</param>
        /// <param name="user">The user joining the tournament.</param>
        /// <returns>A task representing the asynchronous operation. The result is true if the user successfully joins the tournament, otherwise false.</returns>
        Task<bool> JoinTournamentMock(Guid id, ApplicationUser user);
        /// <summary>
        /// Joins a tournament with a transaction reference
        /// </summary>
        /// <param name="id">The ID of the tournament</param>
        /// <param name="user">The user joining the tournament</param>
        /// <param name="transactionReference">The transaction reference</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Returns true if the user successfully joins the tournament, otherwise false</returns>
        Task<bool> JoinTournamentWithTransactionReference(Guid id, ApplicationUser user, string transactionReference = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a tournament.
        /// </summary>
        /// <param name="tournament">The updated tournament details.</param>
        /// <param name="user">The user updating the tournament.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateTournament(TournamentUpdateDTO tournament, ApplicationUser user, CancellationToken cancellationToken = default);
    }
}