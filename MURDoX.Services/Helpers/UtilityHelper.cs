using MURDoX.Data.Factories;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class UtilityHelper
    {
        private readonly AppDbContextFactory _dbFactory;

        public UtilityHelper(AppDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        #region GENERATE RANDOM NUMBERS
        public static int[] MakeRoll(int numDice, int sides)
        {
            int[] numbers = new int[numDice];
            var rnd = new Random();

            for (int i = 0; i < numDice; i++)
            { 
               
                numbers[i] =  rnd.Next(1, sides);
            }
            return numbers;
        }
        #endregion

        #region TUPLE(INT, LIST<INT>) ROLL
        public static (int, List<int>) Roll(int numDice, int sides)
        {
            var nums = new List<int>();
            var result = (numDice, nums);
            var rnd = new Random((int)Environment.TickCount);
            for (int i = 0; i < numDice; i++)
            {
               result.Item2.Add(rnd.Next(1, sides + 1));
            }
            return result;
        }

        #endregion

        #region DICTIONARY<INT, LIST<INT>> DOROLL
        public static Dictionary<int, List<int>> DoRoll(int numDice, int sides)
        {
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            List<int> numbers = new List<int>();
            var rnd = new Random();

            for (int i = 0; i < sides; i++)
            {
                numbers.Add(rnd.Next(1, sides));
            }
            result.Add(numDice, numbers);
            return result;
        }

        #endregion

        #region IS VALID USER
        public bool IsValidUser(ulong id)
        {
            var db = _dbFactory.CreateDbContext();
            var user = db.Users.Where(x => x.DiscordId == id).FirstOrDefault();

            if (user != null)
                return true;

            return false;
            
        }
        #endregion

        #region CREATE NEW SERVER MEMBER
        public async Task CreateNewServerMember(ServerMember member)
        {
            // we are assuming and trusting that a valid ServerMember is being passed in
            //TODO validate member passed in
            var db = _dbFactory.CreateDbContext();
            await db.AddAsync(member);
            await db.SaveChangesAsync();
            
        }
        #endregion

        #region GET DATABASE LATENCY
        public int GetDbLatency()
        {
            var sw = Stopwatch.StartNew();
            var db = _dbFactory.CreateDbContext();
            db.Users.Where(x => x.Username == "Async(void)").FirstOrDefault();
            sw.Stop();
            return (int)sw.ElapsedMilliseconds;
        }

        #endregion
    }
}
