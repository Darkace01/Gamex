using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

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

        var user2 = new ApplicationUser
        {
            Id = "2",
            UserName = "user2",
            DisplayName = "User 2",
            Email = "user2@email.com",
        };
        dbContext.Users.Add(user2);

        var role1 = new IdentityRole
        {
            Id = "1",
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        dbContext.Roles.Add(role1);

        var userRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                UserId = user1.Id,
                RoleId = "1",
            },
            new IdentityUserRole<string>
            {
                UserId = user2.Id,
                RoleId = "1",
            }
        };

        dbContext.UserRoles.AddRange(userRoles);


        // tournament
        List<Tournament> tournaments =
        [
            new Tournament
            {
                Name = "Tournament 1",
                Description = "Description 1",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                EntryFee = 100,
                IsFeatured = true,
                Location = "Location 1",
                Rules = "Rules 1",
            },
            new Tournament
            {
                Name = "Tournament 2",
                Description = "Description 2",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(4),
                EntryFee = 200,
                IsFeatured = false,
                Location = "Location 2",
                Rules = "Rules 2",
            },
            new Tournament
            {
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
            new Picture
            {
                Name = "Picture 1",
                PublicId = "PublicId 1",
                FileUrl = "https://res.cloudinary.com/dzqhcj3km/image/upload/v1629780569/1.jpg",
            },
            new Picture
            {
                Name = "Picture 2",
                PublicId = "PublicId 2",
                FileUrl = "https://res.cloudinary.com/dzqhcj3km/image/upload/v1629780569/2.jpg",
            },
            new Picture
            {
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
            new Post
            {
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
                UserId = dbContext.Users?.AsNoTracking().FirstOrDefault()?.Id,
                PostId = posts.First().Id
            },
            new Comment
            {
                Title = "Comment 2",
                Content = "Comment Content 2",
                UserId = dbContext.Users?.AsNoTracking().FirstOrDefault()?.Id,
                PostId = posts.First().Id
            }
        ];
        dbContext.Comments.AddRange(comments);

        dbContext.SaveChanges();

        //TournamentCategory
        List<TournamentCategory> categories =
        [
            new TournamentCategory
            {
                Name = "Category 1"
            },
            new TournamentCategory
            {
                Name = "Category 2"
            },
            new TournamentCategory
            {
                Name = "Category 3"
            }
        ];

        dbContext.TournamentCategories.AddRange(categories);
        dbContext.SaveChanges();

        // Tag
        List<Tag> tags =
        [
            new Tag
            {
                Name = "Tag 1"
            },
            new Tag
            {
                Name = "Tag 2"
            },
            new Tag
            {
                Name = "Tag 3"
            }
        ];

        dbContext.Tags.AddRange(tags);
        dbContext.SaveChanges();

        // PostTag
        List<PostTag> postTags =
        [
            new PostTag
            {
                PostId = posts.First().Id,
                TagId = tags.First().Id
            },
            new PostTag
            {
                PostId = posts.First().Id,
                TagId = tags.Last().Id
            }
        ];

        dbContext.PostTags.AddRange(postTags);
        dbContext.SaveChanges();

        // PaymentTransaction
        List<PaymentTransaction> paymentTransactions =
        [
            new() {
                UserId = user1.Id,
                Amount = 100,
                Status = TransactionStatus.Success,
                TransactionReference = "TransactionReference 1"
            },
            new ()
            {
                UserId = user1.Id,
                Amount = 200,
                Status = TransactionStatus.Failed,
                TransactionReference = "TransactionReference 2"
            },
            new ()
            {
                UserId = user2.Id,
                Amount = 300,
                Status = TransactionStatus.Pending,
                TransactionReference = "TransactionReference 3",
                TournamentId = tournaments.First().Id
            }
        ];

        dbContext.PaymentTransactions.AddRange(paymentTransactions);
        dbContext.SaveChanges();

        // TournamentRounds
        List<TournamentRound> rounds =
        [
            new TournamentRound
            {
                Name = "Group Stage",
                Description = "Round Description 1",
                TournamentId = tournaments.First().Id
            },
            new TournamentRound
            {
                Name = "Round Of 16",
                Description = "Round Description 2",
                TournamentId = tournaments.First().Id
            },
            new TournamentRound
            {
                Name = "Semi Final",
                Description = "Round Description 3",
                TournamentId = tournaments.Last().Id
            }
        ];
        dbContext.AddRange(rounds);
        dbContext.SaveChanges();

        // RoundMatches

        List<RoundMatch> matches =
        [
            new RoundMatch
            {
                TournamentRoundId = rounds.First().Id,
                Name = "Team A vs Team B",                
            },
            new RoundMatch
            {
                TournamentRoundId = rounds.First().Id,
                Name = "Team C vs Team D",
            },
            new RoundMatch
            {
                TournamentRoundId = rounds.Last().Id,
                Name = "Team E vs Team F",
            }
        ];
        dbContext.AddRange(matches);
        dbContext.SaveChanges();
        // MatchUsers
        List<MatchUser> matchUsers =
        [
            new MatchUser
            {
                MatchId = matches.First().Id,
                UserId = user1.Id
            },
            new MatchUser
            {
                MatchId = matches.First().Id,
                UserId = user2.Id
            }
        ];

        dbContext.AddRange(matchUsers);
        dbContext.SaveChanges();

        return dbContext;
    }
}
