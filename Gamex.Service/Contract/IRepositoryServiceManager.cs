﻿namespace Gamex.Service.Contract
{
    public interface IRepositoryServiceManager
    {
        ITournamentService TournamentService { get; }
        IJWTHelper JWTHelper { get; }
        IPictureService PictureService { get; }
        IFileStorageService FileStorageService { get; }
        IPostService PostService { get; }
        ICommentService CommentService { get; }
        IExtendedUserService ExtendedUserService { get; }
        ITournamentCategoryService TournamentCategoryService { get; }
        ITagService TagService { get; }
        ILeaderboardService LeaderboardService { get; }
        IPaymentService PaymentService { get; }
        IPaystackPayment PaystackPayment { get; }
        IRoundService TournamentRoundService { get; }
        IMatchService RoundMatchService { get; }
        IMatchUserService MatchUserService { get; }
        IDashboardService DashboardService { get; }
        ICacheService CacheService { get; }
    }
}