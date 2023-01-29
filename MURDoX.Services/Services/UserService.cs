using DSharpPlus;
using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        #region GET SERVER MEMBERS
        public async Task GetServerMembersOnStartupAsync(DiscordGuild guild)
        {
            var members = await guild.GetAllMembersAsync();
            var db = _dbFactory.CreateDbContext();
            var dbUsers = db.Users?.ToList();

            foreach (var member in members)
            {
                var currentDbUser = dbUsers?.Where(x => x.Username == member.Username).FirstOrDefault();
                if (currentDbUser is null)
                {
                    var serverMember = new ServerMember()
                    {
                        Username = member.Username,
                        DiscordId = member.Id,
                        Rank = Rank.NEWB,
                        AvatarUrl = member.AvatarUrl,
                        Warnings = 0,
                        Thanks = 0,
                        XP = 0,
                        BankAccountTotal = 0,
                        Created = DateTime.UtcNow,
                    };
                    await db.Users!.AddAsync(serverMember);
                    await db.SaveChangesAsync();
                }
            }
        }
        #endregion

        #region SAVE OR UPDATE USER DICE SCORE
        public async Task<bool> SaveOrUpdateDiceScore(string game, string user, string opponent, int userScore, int opponentScore)
        {
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml", "scores.xml");
            var cs = new CancellationTokenSource();

            if (!File.Exists(xmlPath))
            {
                var doc = new XDocument(
                                new XDeclaration("1.0", "UTF-8", "yes"),
                                    new XElement("scores",
                                        new XElement("game_score",
                                            new XAttribute("game", game),
                                            new XAttribute("user_name", user),
                                            new XAttribute("opponent_name", opponent),
                                            new XAttribute("user_score", userScore),
                                            new XAttribute("opponent_score", opponentScore))));

                var fs = new FileStream(xmlPath, FileMode.Create);
                await doc.SaveAsync(fs, SaveOptions.None, cs.Token);
            }
            else
            {
                var doc = XDocument.Load(xmlPath);
                var scoresElement = new XElement("game_score",
                                            new XAttribute("game", game),
                                            new XAttribute("user_name", user),
                                            new XAttribute("opponent_name", opponent),
                                            new XAttribute("user_score", userScore),
                                            new XAttribute("opponent_score", opponentScore));

                var fs = new FileStream(xmlPath, FileMode.Open);
                doc.Root!.Add(scoresElement);
                await doc.SaveAsync(fs, SaveOptions.None, cs.Token);
            }
            return true;
        }

        #endregion
    }
}
