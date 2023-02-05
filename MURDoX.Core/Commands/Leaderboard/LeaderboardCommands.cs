using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using MURDoX.Core.Comparers;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MURDoX.Core.Commands.Leaderboard
{
    public class LeaderboardCommands : BaseCommandModule
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml", "playerscores.xml");

        [Command("leaderboard")]
        [Description("get a list of game leaders")]
        public async Task Leaderboard(CommandContext ctx)
        {
            var bot = ctx.Client.CurrentUser;
            if (!File.Exists(path))
            {
                await ctx.Channel.SendMessageAsync("leaderboard does not exist!");
            }
            else
            {
                var doc = XDocument.Load(path);
                var players = doc.Root!.Elements("score").ToList();
                List<(string, int, string)> scores = new List<(string, int, string)>();
                var playerStatsBuilder = new StringBuilder();

                foreach (var player in players)
                {
                    var game = player.Attribute("game")!.Value;
                    var playerName = player.Attribute("player_name")!.Value;
                    var currentPlayer = playerName;
                    var wins = int.TryParse(player.Attribute("wins")!.Value, out int currentWins);

                    foreach (var p in players)
                    {
                        if (p.Attribute("player_name")!.Value.Equals(currentPlayer))
                        {
                            currentWins += int.Parse(p.Attribute("wins")!.Value);
                        }
                    }
                    
                    scores.Add((playerName, currentWins, game));
                    scores = scores.Distinct().ToList();
                }

                var sorted = scores.OrderByDescending(x => x.Item2, new TupleComparer<int>());
                var count = 1;

                foreach (var win in sorted)
                {
                     if (count == 1)
                    {
                        playerStatsBuilder.Append($"**Game: {win.Item3.ToUpper()}**\r\n");
                        playerStatsBuilder.Append($"**{count}:** {win.Item1} - Wins: **{win.Item2}**\r\n");
                        count++;
                    }
                     else
                    {
                        playerStatsBuilder.Append($"**{count}:** {win.Item1} - Wins: **{win.Item2}**\r\n");
                        count++;
                    }
                        
                }

                var embedBuilder = new EmbedBuilderHelper();
                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Title = "Leaderboard",
                    Author = ctx.Member.DisplayName,
                    AuthorAvatar = ctx.Member.AvatarUrl,
                    Desc = playerStatsBuilder.ToString(),
                    Footer = "MURDoX",
                    TimeStamp= DateTime.Now,
                    FooterImgUrl = bot.AvatarUrl,
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
        }
    }
}
