using Microsoft.EntityFrameworkCore;
using MURDoX.Data.Factories;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public abstract class LevelHelper
    {
       
        public static async Task<int> GetXp(string member) 
        { 
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            var user = db.Users!.Where(x => x.Username== member).FirstOrDefault();

            if (user != null)
            {
                var xp = await db.Users!.Where(x => x.Username== member).Select(x => x.XP).FirstOrDefaultAsync();
                return xp;
            }

            return 0;
        }

        public static void SetXp(string member, int amount)
        {
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            var user = db.Users!.Where(x => x.Username == member).FirstOrDefault();

            if (user != null)
            {
                var xp = db.Users!.Where(x => x.Username == member).Select(x => x.XP).FirstOrDefault();
                xp += amount;
                user.XP = xp;
                user.Created = DateTime.UtcNow;
                try
                {
                    db.Update(user);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }
           

        }

        public static Rank GetRank(string member) 
        { 
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            var rank = db.Users!.Where(x => x.Username == member).Select(x => x.Rank).FirstOrDefault();

            return rank;
        }

        public static void SetRank(string member, Rank rank)
        {
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            var user = db.Users!.Where(x => x.Username == member).Select(x => x).FirstOrDefault();
            user!.Rank = rank;

            db.Update(user); 
            db.SaveChanges();
        }

        #region GET THANKS
        public static async Task<int> GetThanks(string username)
        {
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            var thanks = await db.Users!.Where(x => x.Username == username).Select(x => x.Thanks).FirstOrDefaultAsync();

            return thanks;
        }
        #endregion
    }
}
