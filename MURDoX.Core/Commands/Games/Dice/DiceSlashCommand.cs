using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using System.Text;

namespace MURDoX.Core.Commands.Games.Dice
{
    public class DiceSlashCommand : ApplicationCommandModule
    {
        
        [SlashCommand("dice", "starts a new dice game with 2 players")]
        public async Task DiceCommand(InteractionContext ctx,
                        [Option("opponent", "opponent")] string opponent)
        {
            var bot = ctx.Client.CurrentUser;
            var playerOne = ctx.Member.Username.Trim();
            var gameId = Guid.NewGuid();
            var gameName = $"Dice-Roller: Game:{playerOne}";

            var game = new Game()
            {
                GameId = gameId,
                Players = new List<(string, int)> { (playerOne, 0), (opponent.Trim(), 0) },
                GameName = gameName,
                IsRunning = true
            };
           
            var newGame = GameService.StartNewGame(game);
            var interactionResponseBuilder = new DiscordInteractionResponseBuilder();
            var followUpBuilder = new DiscordFollowupMessageBuilder();
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: interactionResponseBuilder.WithContent("MURDoX is initializing **Dice Roller**."));
            await Task.Delay(2000);
            if (newGame.GameName.Equals(""))
            {
                try
                {
                    await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"Game already in progress, please choose a new Game Name"));
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
                await Task.Delay(2000);
                return;
            }
            else
            {
                GameService.IsRunning = true;
                await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"dice roller started! with ``{game.Players.First().Player}[{game.Players[0].Wins}]`` vs ``{game.Players[1].Player}[{game.Players[1].Wins}]``"));
                await Task.Delay(2000);
            }
            var rnd = new Random();
            while(GameService.IsRunning)
            {
                var playerOneNums = UtilityHelper.Roll(6, 6);
                var playerOneTotal = playerOneNums.Item2.Sum();
                await Task.Delay(1000);
                var playerTwoNums = UtilityHelper.Roll(6, 6);
                var playerTwoTotal = playerTwoNums.Item2.Sum();
                var playerOneNumBuilder = new StringBuilder();
                var playerTwoNumBuilder = new StringBuilder();
                var count = 0;
                foreach (var n in playerOneNums.Item2)
                {
                    
                    if (count == 5)
                    {
                        playerOneNumBuilder.Append(n.ToString());
                        count = 0;
                    }               
                    else
                    {
                        playerOneNumBuilder.Append(n.ToString() + ',');
                        count++;
                    }
                       
                }
                foreach (var p in playerTwoNums.Item2)
                {
                   
                    if (count == 5)
                    {
                         playerTwoNumBuilder.Append(p.ToString());
                        count = 0;
                    } 
                    else
                    {
                        playerTwoNumBuilder.Append(p.ToString() + ',');
                        count++;
                    }
                        
                }
               
                var winner = "";
                if (playerOneTotal > playerTwoTotal)
                {
                    winner = game.Players[0].Player;
                    GameService.SavePlayerScoreToXml(game.Players[0].Player, 1, "dice");
                    //GameService.SavePlayerScoreToXml(game.Players[1].Player, 0);
                }
                    
                else if (playerTwoTotal > playerOneTotal)
                {
                    winner = game.Players[1].Player;
                    GameService.SavePlayerScoreToXml(game.Players[1].Player, 1, "dice");
                    //GameService.SavePlayerScoreToXml(game.Players[0].Player, 0);
                }
                else
                {
                    winner = "Draw";
                }
                    
                await Task.Delay(500);
                
                var embedBuilder = new EmbedBuilderHelper();
                var fields = new EmbedField[]
                {
                    new EmbedField() { Name = "Player", Value = game.Players[0].Player, Inline = true },
                    new EmbedField() { Name = "Roll", Value = playerOneNumBuilder.ToString(), Inline = true },
                    new EmbedField() { Name = "Total", Value = playerOneTotal.ToString(), Inline = true },
                    new EmbedField() { Name = "Player", Value = game.Players[1].Player, Inline = true },
                    new EmbedField() { Name = "Roll", Value = playerTwoNumBuilder.ToString(), Inline = true },
                    new EmbedField() { Name = "Total", Value = playerTwoTotal.ToString(), Inline = true },
                    new EmbedField() { Name = "Winner", Value = winner, Inline = true },
                };

                var embed = new Embed()
                {
                    AuthorAvatar = bot.AvatarUrl,
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Title = "Dice Roller",
                    Fields = fields,
                    Footer = $"MURDoX",
                    TimeStamp = DateTime.Now,
                    FooterImgUrl = bot.AvatarUrl,
                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                int delay = rnd.Next(10, 16) * 1000;
                await Task.Delay(delay);
            }
         
            //await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"{game.GameName}"));

        }

        [SlashCommand("stopdice", "stops the dice roller game")]
        public async Task StopCommand(InteractionContext ctx)
        {
            GameService.IsRunning = false;
            GameService.ClearGames();
            var followUpBuilder = new DiscordFollowupMessageBuilder();
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, 
                builder: new DiscordInteractionResponseBuilder().WithContent("MURDoX is stopping **Dice Roller**"));
            await Task.Delay(2000);
            await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"**Dice Roller** has been stopped by ``{ctx.Member.Username}``"));

        }
    }
}
