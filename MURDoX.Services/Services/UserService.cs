using MURDoX.Data.Factories;
using MURDoX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContextFactory _dbFactory;

        public UserService(AppDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task UpdateOrAddXpToUser(string user, int amount)
        {
            var db = _dbFactory.CreateDbContext();
            var member = db.Users.Where(x => x.Username == user).FirstOrDefault();
            var xp = member?.XP;
            member!.XP = (int)xp! + amount;
            await db.SaveChangesAsync();
        }
    }
}
