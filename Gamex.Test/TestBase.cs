namespace Gamex.Test;

public class TestBase
{
    protected GamexDbContext GetSampleData(string dbName)
    {
        var options = new DbContextOptionsBuilder<GamexDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var dbContext = new GamexDbContext(options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        //user manager


        // Add sample data

        // user
        var user1 = new ApplicationUser
        {
            Id = "1",
            UserName = "user1",
            DisplayName = "User 1",
            Email = "user1@email.com",
        };
        dbContext.Users.Add(user1);


        // tournament
        List<Tournament> tournaments =
        [
            new Tournament {
                Name = "Tournament 1",
                Description = "Description 1",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                EntryFee = 100,
                IsFeatured = true,
                Location = "Location 1",
                Rules = "Rules 1",                
            },
            new Tournament {
                Name = "Tournament 2",
                Description = "Description 2",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(4),
                EntryFee = 200,
                IsFeatured = false,
                Location = "Location 2",
                Rules = "Rules 2",
            },
            new Tournament {
                Name = "Tournament 3",
                Description = "Description 3",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(6),
                EntryFee = 300,
                IsFeatured = true,
                Location = "Location 3",
                Rules = "Rules 3",
            },
        ];

        dbContext.Tournaments.AddRange(tournaments);
        dbContext.SaveChanges();

        // user tournament
        var savedTournaments = dbContext.Tournaments.AsNoTracking().ToList();
        foreach (var tournament in savedTournaments)
        {
            dbContext.UserTournaments.Add(new UserTournament
            {
                TournamentId = tournament.Id,
                UserId = user1.Id,
            });
        }

        // picture
        List<Picture> pictures = [
            new Picture {
                Name = "Picture 1",
                PublicId = "PublicId 1",
                FileUrl = "https://res.cloudinary.com/dzqhcj3km/image/upload/v1629780569/1.jpg",
            },
            new Picture {
                Name = "Picture 2",
                PublicId = "PublicId 2",
                FileUrl = "https://res.cloudinary.com/dzqhcj3km/image/upload/v1629780569/2.jpg",
            },
            new Picture {
                Name = "Picture 3",
                PublicId = "PublicId 3",
                FileUrl = "https://res.cloudinary.com/dzqhcj3km/image/upload/v1629780569/3.jpg",
            },
        ];

        dbContext.Pictures.AddRange(pictures);
        dbContext.SaveChanges();

        // Post
        List<Post> posts =
        [
            new Post {
               Title = "Post 1",
               Content = "Post Content 1",
               PictureId = dbContext.Pictures.AsNoTracking().FirstOrDefault()?.Id,
               UserId = dbContext.Users.AsNoTracking().FirstOrDefault()?.Id
             },
            new Post
            {
               Title = "Post 2",
               Content = "Post Content 2",
               PictureId = dbContext.Pictures.AsNoTracking().FirstOrDefault()?.Id,
               UserId = dbContext.Users.AsNoTracking().FirstOrDefault()?.Id
            }
        ];

        dbContext.Posts.AddRange(posts);
        dbContext.SaveChanges();

        //Comment
        List<Comment> comments =
        [
            new Comment
            {
                Title = "Comment 1",
                Content = "Comment Content 1",
                UserId =  dbContext.Users.AsNoTracking().FirstOrDefault()?.Id,
                PostId = posts.First().Id
            },
            new Comment
            {
                Title = "Comment 2",
                Content = "Comment Content 2",
                UserId =  dbContext.Users.AsNoTracking().FirstOrDefault()?.Id,
                PostId = posts.First().Id
            }
        ];
        dbContext.Comments.AddRange(comments);

        dbContext.SaveChanges();

        return dbContext;

    }
}
