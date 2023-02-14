using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Utility.SuggestionService;

namespace MURDoX.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Log>? Logs { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<ServerMember>? Users { get; set; }
        public DbSet<Reminder>? Reminders { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override DatabaseFacade Database => base.Database;
       
    }
}