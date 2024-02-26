namespace Gamex.Service.Contract
{
    public interface IRepositoryServiceManager
    {
        ITournamentService TournamentService { get; }
        IJWTHelper JWTHelper { get; }
        //ISMTPMailService SMTPMailService { get; }
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
    }
}