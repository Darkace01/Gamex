﻿using Gamex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gamex.Data;

public class GamexDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<UserTournament> UserTournaments { get; set; }

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
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<UserTournament>()
            .HasOne(ut => ut.Tournament)
            .WithMany(t => t.UserTournaments)
            .HasForeignKey(ut => ut.TournamentId)
            .OnDelete(DeleteBehavior.NoAction);

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

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Picture);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Picture)
            .WithOne(p => p.User)
            .HasForeignKey<Picture>(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}