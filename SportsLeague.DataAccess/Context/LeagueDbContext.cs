using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context
{
    // Contexto principal de la base de datos
    // Aquí se configuran las entidades y sus relaciones
    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options)
        {
        }

        // ── DbSets (Tablas) ──
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Referee> Referees => Set<Referee>();
        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // TEAM
            // =========================
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(t => t.City)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(t => t.Stadium)
                      .HasMaxLength(150);

                entity.Property(t => t.LogoUrl)
                      .HasMaxLength(500);

                entity.HasIndex(t => t.Name)
                      .IsUnique();
            });

            // =========================
            // PLAYER
            // =========================
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(80);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(80);
                entity.Property(p => p.BirthDate).IsRequired();
                entity.Property(p => p.Number).IsRequired();
                entity.Property(p => p.Position).IsRequired();

                // Relación con Team (1:N)
                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Players)
                      .HasForeignKey(p => p.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Un jugador no puede repetir número en el mismo equipo
                entity.HasIndex(p => new { p.TeamId, p.Number })
                      .IsUnique();
            });

            // =========================
            // REFEREE
            // =========================
            modelBuilder.Entity<Referee>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.FirstName).IsRequired().HasMaxLength(80);
                entity.Property(r => r.LastName).IsRequired().HasMaxLength(80);
                entity.Property(r => r.Nationality).IsRequired().HasMaxLength(80);
            });

            // =========================
            // TOURNAMENT
            // =========================
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name).IsRequired().HasMaxLength(150);
                entity.Property(t => t.Season).IsRequired().HasMaxLength(20);
                entity.Property(t => t.StartDate).IsRequired();
                entity.Property(t => t.EndDate).IsRequired();
                entity.Property(t => t.Status).IsRequired();
            });

            // =========================
            // TOURNAMENT TEAM (N:M)
            // =========================
            modelBuilder.Entity<TournamentTeam>(entity =>
            {
                entity.HasKey(tt => tt.Id);

                entity.Property(tt => tt.RegisteredAt).IsRequired();

                entity.HasOne(tt => tt.Tournament)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tt => tt.Team)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                      .IsUnique();
            });

            // =========================
            // SPONSOR
            // =========================
            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.ContactEmail)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.Phone)
                      .HasMaxLength(20);

                entity.Property(s => s.WebsiteUrl)
                      .HasMaxLength(500);

                entity.HasIndex(s => s.Name)
                      .IsUnique(); // evita duplicados
            });

            // =========================
            // TOURNAMENT SPONSOR (N:M con datos extra)
            // =========================
            modelBuilder.Entity<TournamentSponsor>(entity =>
            {
                entity.HasKey(ts => ts.Id);

                entity.HasOne(ts => ts.Tournament)
                      .WithMany(t => t.TournamentSponsors)
                      .HasForeignKey(ts => ts.TournamentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ts => ts.Sponsor)
                      .WithMany(s => s.TournamentSponsors)
                      .HasForeignKey(ts => ts.SponsorId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Evita repetir sponsor en el mismo torneo
                entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
                      .IsUnique();

                entity.Property(ts => ts.ContractAmount)
                      .HasPrecision(18, 2);

                entity.Property(ts => ts.JoinedAt)
                      .IsRequired();
            });
        }
    }
}