#region

using System.Diagnostics;
using System.Text.RegularExpressions;
using MURDoX.Core.Data;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Utility.SuggestionService;

#endregion

namespace MURDoX.Core.Helpers
{
    public class UtilityHelper
    {
        private readonly ApplicationDbContext _db;
        //private readonly ILoggerService _logger;

        public UtilityHelper(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region GENERATE RANDOM NUMBERS

        public static int[] MakeRoll(int numDice, int sides)
        {
            int[] numbers = new int[numDice];
            Random rnd = new();

            for (int i = 0; i < numDice; i++) numbers[i] = rnd.Next(1, sides);
            return numbers;
        }

        #endregion

        #region TUPLE(INT, LIST<INT>) ROLL

        public static (int, List<int>) Roll(int numDice, int sides)
        {
            List<int> nums = new();
            (int numDice, List<int> nums) result = (numDice, nums);
            Random rnd = new(Environment.TickCount);
            for (int i = 0; i < numDice; i++) result.Item2.Add(rnd.Next(1, sides + 1));
            return result;
        }

        #endregion

        #region DICTIONARY<INT, LIST<INT>> DOROLL

        public static Dictionary<int, List<int>> DoRoll(int numDice, int sides)
        {
            Dictionary<int, List<int>> result = new();
            List<int> numbers = new();
            Random rnd = new();

            for (int i = 0; i < sides; i++) numbers.Add(rnd.Next(1, sides));
            result.Add(numDice, numbers);
            return result;
        }

        #endregion

        #region IS VALID USER

        public bool IsValidUser(ulong id)
        {
            ServerMember? user = _db.Users!.Where(x => x.DiscordId == id).FirstOrDefault();

            if (user != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region CREATE NEW SERVER MEMBER

        public async Task CreateNewServerMember(ServerMember member)
        {
            // we are assuming and trusting that a valid ServerMember is being passed in
            //TODO validate member passed in

            await _db.AddAsync(member);
            await _db.SaveChangesAsync();
            Console.WriteLine($"[INFO]  Server Member {member.Username} Created [{DateTimeOffset.UtcNow}]");
        }

        #endregion

        #region GET DATABASE LATENCY

        public int GetDbLatency()
        {
            Stopwatch sw = Stopwatch.StartNew();
            _db.Users!.FirstOrDefault(x => x.Username == "Async(void)");
            sw.Stop();
            return (int)sw.ElapsedMilliseconds;
        }

        #endregion

        #region SAVE SUGGESTION

        public void SaveSuggestionToDb(Suggestion suggestion)
        {
            _db.Suggestions!.Add(suggestion);
            _db.SaveChanges();
        }

        #endregion

        #region SANATIZE

        /// <summary>
        ///     Sanatize a string, take out all html that start with '&' and ends with ';'
        /// </summary>
        /// <param name="content"></param>
        /// <returns>string</returns>
        public static string Sanitize(string content)
        {
            Regex matches = new("(&#?[a-zA-Z0-9]+;)");
            string output = matches.Replace(content, "");

            return output;
        }

        #endregion
    }
}