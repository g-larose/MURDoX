#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Games.EconomyGame.ContextModels;
using MURDoX.Core.Models.Utility.SuggestionService;

#endregion

namespace MURDoX.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<Log>? Logs { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<ServerMember>? Users { get; set; }
        public DbSet<Reminder>? Reminders { get; set; }
        
        // Economy
        public DbSet<EconomyPlanet> EconomyPlanets { get; set; }
        public DbSet<EconomyPlayers> EconomyPlayers { get; set; }
        public DbSet<EconomySettings> EconomySettings { get; set; }
        // Economy - End
        public override DatabaseFacade Database => base.Database;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}