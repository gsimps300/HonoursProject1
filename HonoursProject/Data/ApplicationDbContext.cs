using HonoursProject.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<GamePlatformMapping> GamePlatformMappings { get; set; }
    public DbSet<LobbyParticipant> LobbyParticipants { get; set; } // ✅ New table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Ensure Email and UserName are unique
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();

        // ✅ Configure many-to-many relationship between Game and GamePlatform
        modelBuilder.Entity<GamePlatformMapping>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.PlatformMappings)
            .HasForeignKey(gp => gp.GameId);

        // ✅ Configure many-to-many relationship using LobbyParticipant
        modelBuilder.Entity<LobbyParticipant>()
            .HasKey(lp => new { lp.LobbyId, lp.UserId });

        modelBuilder.Entity<LobbyParticipant>()
            .HasOne(lp => lp.Lobby)
            .WithMany(l => l.Participants)
            .HasForeignKey(lp => lp.LobbyId);

        modelBuilder.Entity<LobbyParticipant>()
            .HasOne(lp => lp.User)
            .WithMany(u => u.LobbiesJoined)
            .HasForeignKey(lp => lp.UserId);
    }
}


