namespace Gamex.Service.Implementation;

public class RepositoryServiceManager : IRepositoryServiceManager
{
    private readonly GamexDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly Cloudinary _cloudinary;

    private ITournamentService _tournamentService;
    private IJWTHelper _jwtHelper;
    //private ISMTPMailService _smtpMailService;
    private IPictureService _pictureService;
    private IFileStorageService _fileStorageService;
    private IPostService _postService;
    private ICommentService _commentService;
    private IExtendedUserService _extendedUserService;
    private ITournamentCategoryService _tournamentCategoryService;
    private ITagService _tagService;
    private ILeaderboardService _leaderboardService;
    private IPaymentService _paymentService;

    public RepositoryServiceManager(GamexDbContext context, IConfiguration config)
    {
        _context = context;
        _configuration = config;
        _cloudinary = new Cloudinary(_configuration["Cloudinary:Url"]);
    }

    public ITournamentService TournamentService
    {
        get
        {
            _tournamentService ??= new TournamentService(_context);
            return _tournamentService;
        }
    }

    public IJWTHelper JWTHelper
    {
        get
        {
            _jwtHelper ??= new JWTHelper(_configuration);
            return _jwtHelper;
        }
    }

    //public ISMTPMailService SMTPMailService
    //{
    //    get
    //    {
    //        _smtpMailService ??= new SMTPMailService(_configuration);
    //        return _smtpMailService;
    //    }
    //}

    public IPictureService PictureService
    {
        get
        {
            _pictureService ??= new PictureService(_context);
            return _pictureService;
        }
    }

    public IFileStorageService FileStorageService
    {
        get
        {
            _fileStorageService ??= new FileStorageService(_cloudinary);
            return _fileStorageService;
        }
    }

    public IPostService PostService
    {
        get
        {
            _postService ??= new PostService(_context);
            return _postService;
        }
    }

    public ICommentService CommentService
    {
        get
        {
            _commentService ??= new CommentService(_context);
            return _commentService;
        }
    }

    public IExtendedUserService ExtendedUserService
    {
        get
        {
            _extendedUserService ??= new ExtendedUserService(_context);
            return _extendedUserService;
        }
    }

    public ITournamentCategoryService TournamentCategoryService
    {
        get
        {
            _tournamentCategoryService ??= new TournamentCategoryService(_context);
            return _tournamentCategoryService;
        }
    }

    public ITagService TagService
    {
        get
        {
            _tagService ??= new TagService(_context);
            return _tagService;
        }
    }

    public ILeaderboardService LeaderboardService
    {
        get
        {
            _leaderboardService ??= new LeaderboardService(_context);
            return _leaderboardService;
        }
    }

    public IPaymentService PaymentService
    {
        get
        {
            _paymentService ??= new PaymentService(_context);
            return _paymentService;
        }
    }
}
