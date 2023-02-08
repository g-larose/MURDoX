using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Log>? Logs { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<ServerMember>? Users { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
       
            
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //public override DatabaseFacade Database => base.Database;
       
    }
}
