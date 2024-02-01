using Gamex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gamex.Data;

public class GamexDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<UserTournament> UserTournaments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TournamentCategory> TournamentCategories { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public GamexDbContext(DbContextOptions<GamexDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserTournament>()
            .HasKey(ut => new { ut.UserId, ut.TournamentId });

        builder.Entity<UserTournament>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTournaments)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Changed from NoAction to Cascade

        builder.Entity<UserTournament>()
            .HasOne(ut => ut.Tournament)
            .WithMany(t => t.UserTournaments)
            .HasForeignKey(ut => ut.TournamentId)
            .OnDelete(DeleteBehavior.Cascade); // Changed from NoAction to Cascade

        builder.Entity<UserTournament>()
            .Property(ut => ut.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Tournament>()
            .HasOne(t => t.Picture);

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
            .OnDelete(DeleteBehavior.Cascade); // Added new foreign key constraint
            
        builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction); // Added new foreign key constraint

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Picture);

        builder.Entity<PaymentTransaction>()
            .HasOne(pt => pt.User)
            .WithMany(u => u.PaymentTransactions)
            .HasForeignKey(pt => pt.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Added new foreign key constraint

        builder.Entity<PaymentTransaction>()
            .HasOne(pt => pt.Tournament)
            .WithMany(t => t.PaymentTransactions)
            .HasForeignKey(pt => pt.TournamentId)
            .OnDelete(DeleteBehavior.NoAction); // Added new foreign key constraint

        builder.Entity<PaymentTransaction>()
            .Property(pt => pt.Amount)
            .HasColumnType("decimal(18,2)");
        
    }
}
