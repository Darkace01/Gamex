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
        List<Tournament> tournaments = new()
        {
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
        };

        dbContext.Tournaments.AddRange(tournaments);

        // picture
        List<Picture> pictures = new(){
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
        };

        dbContext.Pictures.AddRange(pictures);

        dbContext.SaveChanges();

        return dbContext;

    }
}
