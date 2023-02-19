#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Utility.SuggestionService;

#endregion

namespace MURDoX.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //Database.Migrate();
            
        }

        public DbSet<Log>? Logs { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<ServerMember>? Users { get; set; }
        public DbSet<Reminder>? Reminders { get; set; }

        public override DatabaseFacade Database => base.Database;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}