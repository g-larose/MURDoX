using DSharpPlus.CommandsNext.Attributes;
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
                        [Option("opponent", "opponent")] DiscordUser user)
        {


            var bot = ctx.Client.CurrentUser;
            var playerOne = ctx.Member;
            var playerTwo = user;
            var embed = new Embed();
            var embedBuilder = new EmbedBuilderHelper();

            if (playerOne.Username.Equals(playerTwo.Username))
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"I won't let you create a new **Dice Roller** game against yourself.",
                };
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: new DiscordInteractionResponseBuilder().AddEmbed(embed: embedBuilder.Build(embed)));
                return;
            }
            var gameId = Guid.NewGuid();
            var gameName = $"Dice-Roller: Game:{playerOne.Username.Trim()}";

            var game = new Game()
            {
                GameId = gameId,
                Players = new List<(string, int)> { (playerOne.Username.Trim(), 0), (playerTwo.Username.ToString(), 0) },
                GameName = gameName,
                IsRunning = true
            };

            if (GameService.IsValidGame(game.GameName))
            {
                //TODO: dont start a new dice game
            }
            var newGame = GameService.StartNewGame(game);
            var interactionResponseBuilder = new DiscordInteractionResponseBuilder();
            var followUpBuilder = new DiscordFollowupMessageBuilder();
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: interactionResponseBuilder.WithContent("MURDoX is initializing **Dice Roller**."));
            await Task.Delay(2000);
            if (newGame.GameName.Equals(""))
            {
                try
                {
                    embed = new Embed()
                    {
                        Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                        Desc = "Game already in progress, I won't let you start a new *Dice Roller* game against the same opponent",
                    };
                    await ctx.FollowUpAsync(builder: followUpBuilder.AddEmbed(embed: embedBuilder.Build(embed)));
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
                var playerOneWins = GameService.GetPlayerScore(playerOne.Username);
                var playerTwoWins = GameService.GetPlayerScore(playerTwo.Username);
                await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"dice roller started! with ``{game.Players[0].Player}[{playerOneWins}]`` vs ``{game.Players[1].Player}[{playerTwoWins}]``"));
                await Task.Delay(2000);
            }
            var rnd = new Random();
            while(GameService.IsRunning)
            {
                foreach (var g in GameService.games)
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

                    embed = new Embed()
                    {
                        AuthorAvatar = bot.AvatarUrl,
                        Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                        Title = "Dice Roller",
                        Desc = $"``Game:{gameId}``",
                        ThumbnailImgUrl = "https://i.imgur.com/zZWHhWY.png",
                        Fields = fields,
                        Footer = $"MURDoX",
                        TimeStamp = DateTime.Now,
                        FooterImgUrl = bot.AvatarUrl,
                    };
                    await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                    int coolDown = rnd.Next(10, 16) * 1000;
                    await Task.Delay(coolDown);
                }
            }
         
            //await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"{game.GameName}"));

        }

        [SlashCommand("stopdice", "stops the dice roller game with the author's Username")]
        public async Task StopCommand(InteractionContext ctx)
        {
            var gameId = GameService.GetGameId($"Dice-Roller: Game:{ctx.Member.Username}");
            var embedBuilder = new EmbedBuilderHelper();
            Embed embed;
            if (gameId.Equals(Guid.Empty)) // a member that doesn't have a game in progress called 'stopdice' so we ignore it
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"I couldn't find a game named **[Dice-Roller: Game:{ctx.Member.Username}]** to stop."
                };
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
                        builder: new DiscordInteractionResponseBuilder().AddEmbed(embed: embedBuilder.Build(embed)));
                return; 
            }
               

            var gameToStop = GameService.games.Find(x => x.GameId.Equals(gameId));
            var index = GameService.games.IndexOf(gameToStop!);
            GameService.games.Remove(GameService.games.ElementAt(index));
            embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = $"MURDoX is stopping **{gameToStop!.GameName}**"
            };
           
            var followUpBuilder = new DiscordFollowupMessageBuilder();
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, 
                builder: new DiscordInteractionResponseBuilder().AddEmbed(embed: embedBuilder.Build(embed)));
            await Task.Delay(2000);

            embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = $"**Game: {gameToStop!.GameId}** has been stopped by ``{ctx.Member.Username}``"
            };
            await ctx.FollowUpAsync(builder: followUpBuilder.AddEmbed(embed: embedBuilder.Build(embed)));

        }

        [SlashCommand("stopalldice", "stops all the current dice roller games")]
        [RequirePermissions(DSharpPlus.Permissions.Administrator)]
        public async Task StopAllDice(InteractionContext ctx)
        {
            var embedBuilder = new EmbedBuilderHelper();
            if (GameService.games.Count > 0) 
            {
                GameService.IsRunning = false;
                GameService.games.Clear();
                
                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"All Dice Roller games have been stopped by: **{ctx.Member.Username}**",
                };
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
                    builder: new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder.Build(embed)));
            }
            else
            {
                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"there are **[0]** Dice Roller games running",
                };
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
                    builder: new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder.Build(embed)));
            }

          
        }
    }
}
