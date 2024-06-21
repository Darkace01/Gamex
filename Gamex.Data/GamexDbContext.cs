using Gamex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gamex.Data;

public class GamexDbContext(DbContextOptions<GamexDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<UserTournament> UserTournaments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TournamentCategory> TournamentCategories { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<UserConfirmationCode> UserConfirmationCodes { get; set; }
    public DbSet<TournamentRound> TournamentRounds { get; set; }
    public DbSet<RoundMatch> RoundMatches { get; set; }
    public DbSet<MatchUser> MatchUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Soft Delete Filter
        #region Soft Delete Filter
        builder.Entity<Tournament>().HasQueryFilter(t => !t.IsDeleted);
        builder.Entity<Picture>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<UserTournament>().HasQueryFilter(ut => !ut.IsDeleted);
        builder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<TournamentCategory>().HasQueryFilter(tc => !tc.IsDeleted);
        builder.Entity<PaymentTransaction>().HasQueryFilter(pt => !pt.IsDeleted);
        builder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);
        builder.Entity<PostTag>().HasQueryFilter(pt => !pt.IsDeleted);
        builder.Entity<UserConfirmationCode>().HasQueryFilter(ucc => !ucc.IsDeleted);
        builder.Entity<TournamentRound>().HasQueryFilter(tr => !tr.IsDeleted);
        builder.Entity<RoundMatch>().HasQueryFilter(rm => !rm.IsDeleted);
        builder.Entity<MatchUser>().HasQueryFilter(mu => !mu.IsDeleted);
        #endregion

        // Indexes
        const string IsDeletedIndex = "IsDeleted = 0";
        #region Indexes
        builder.Entity<Tournament>()
            .HasIndex(t => t.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<Picture>()
            .HasIndex(p => p.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<UserTournament>()
            .HasIndex(ut => ut.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<Post>()
            .HasIndex(p => p.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<Comment>()
            .HasIndex(c => c.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<TournamentCategory>()
            .HasIndex(tc => tc.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<PaymentTransaction>()
            .HasIndex(pt => pt.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<Tag>()
            .HasIndex(t => t.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<PostTag>()
            .HasIndex(pt => pt.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<UserConfirmationCode>()
            .HasIndex(ucc => ucc.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<TournamentRound>()
            .HasIndex(tr => tr.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<RoundMatch>()
            .HasIndex(rm => rm.IsDeleted)
            .HasFilter(IsDeletedIndex);

        builder.Entity<MatchUser>()
            .HasIndex(mu => mu.IsDeleted)
            .HasFilter(IsDeletedIndex);
        #endregion

        builder.Entity<UserTournament>()
            .HasKey(ut => new { ut.UserId, ut.TournamentId });

        builder.Entity<UserTournament>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTournaments)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Changed from NoAction to Cascade

        builder.Entity<UserTournament>()
            .HasOne(ut => ut.Tournament)
            .WithMany(t => t.UserTournaments)
            .HasForeignKey(ut => ut.TournamentId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Changed from NoAction to Cascade

        builder.Entity<UserTournament>()
            .Property(ut => ut.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Tournament>()
            .HasOne(t => t.Picture)
            .WithMany()
            .HasForeignKey(t => t.PictureId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<Tournament>()
            .Property(t => t.EntryFee)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Tournament>()
            .Property(t => t.Location)
            .HasMaxLength(300);

        builder.Entity<Tournament>()
            .Property(t => t.Name)
            .HasMaxLength(300);

        builder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Added new foreign key constraint

        builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Added new foreign key constraint

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Picture);

        builder.Entity<PaymentTransaction>()
            .HasOne(pt => pt.User)
            .WithMany(u => u.PaymentTransactions)
            .HasForeignKey(pt => pt.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Added new foreign key constraint

        builder.Entity<PaymentTransaction>()
            .HasOne(pt => pt.Tournament)
            .WithMany(t => t.PaymentTransactions)
            .HasForeignKey(pt => pt.TournamentId)
            .OnDelete(DeleteBehavior.ClientNoAction); // Added new foreign key constraint

        builder.Entity<PaymentTransaction>()
            .Property(pt => pt.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        builder.Entity<PostTag>()
           .HasKey(ut => new { ut.PostId, ut.TagId });

        builder.Entity<Tag>()
            .HasMany(t => t.PostTags)
            .WithOne(pt => pt.Tag)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<Post>()
            .HasMany(p => p.PostTags)
            .WithOne(pt => pt.Post)
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<TournamentRound>()
            .HasOne(tr => tr.Tournament)
            .WithMany(t => t.TournamentRounds)
            .HasForeignKey(tr => tr.TournamentId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<Tournament>()
            .HasMany(t => t.TournamentRounds)
            .WithOne(tr => tr.Tournament)
            .HasForeignKey(tr => tr.TournamentId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<RoundMatch>()
            .HasOne(rm => rm.TournamentRound)
            .WithMany(tr => tr.RoundMatches)
            .HasForeignKey(rm => rm.TournamentRoundId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.Entity<TournamentRound>()
            .HasMany(tr => tr.RoundMatches)
            .WithOne(rm => rm.TournamentRound)
            .HasForeignKey(rm => rm.TournamentRoundId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}
