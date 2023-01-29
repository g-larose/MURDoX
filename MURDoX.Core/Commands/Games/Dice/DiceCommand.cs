using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Games.Dice
{
    public class DiceCommand : BaseCommandModule
    {

        #region ROLL DICE

        [Command("dice")]
        [Description("rolls a set of dice")]
        public async Task RollDice(CommandContext ctx, [RemainingText] string args = "6 6")
        {
            var commandArgs = args.Split(' '); //format !dice [dice] [sides]  example: !dice 2 6
            var bot = ctx.Client.CurrentUser;
            var messageAuthor = ctx.Message.Author;

            if (commandArgs.Length > 2 || commandArgs.Length < 2)
            {
                await ctx.Channel.SendMessageAsync("``invalid arguments, please try again!``");
            }
            else
            {
                if (int.TryParse(commandArgs[0], out int numDice))
                {
                    if (int.TryParse(commandArgs[1], out int sides))
                    {
                        //check to see if the numDice and sides are legal numbers
                        if (numDice < 7 && numDice > 0 && sides < 13 && sides > 0 )
                        {
                            //commandArgs are good
                            //create embed to send to chat with the MakeRoll results.
                            var nums = UtilityHelper.Roll(numDice, sides);
                            var dieBuilder = new StringBuilder();
                            dieBuilder.Append(String.Join(" ", nums.Item2));

                            var score = dieBuilder.ToString().Trim().Split(" ")
                                                  .Select(x => int.Parse(x))
                                                  .Sum();


                            var embedBuilder = new EmbedBuilderHelper();
                            var dieField = new EmbedField() { Name = "Dice", Value = numDice.ToString(), Inline = true };
                            var numField = new EmbedField() { Name = "Values", Value = dieBuilder.ToString(), Inline = true };
                            var scoreField = new EmbedField() { Name = "Score", Value = score.ToString(), Inline = true };

                            var fields = new EmbedField[] { dieField, numField, scoreField };
                            
                            var embed = new Embed()
                            {
                                Author = messageAuthor.Username,
                                AuthorAvatar = messageAuthor.AvatarUrl,
                                Desc = $"{messageAuthor.Username} rolled **{numDice}** dice with **{sides}** sides each!",
                                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                                ThumbnailImgUrl = "https://i.imgur.com/zZWHhWY.png",
                                Fields = fields,
                                TimeStamp = DateTime.Now,
                                Footer = $"{bot.Username}"
                            };
                            await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        }
                        else
                             await ctx.Channel.SendMessageAsync("invalid arguments, please try again!");
                        

                    }
                    else
                        await ctx.Channel.SendMessageAsync("invalid arguments, please try again!");
                }
                else
                    await ctx.Channel.SendMessageAsync("invalid arguments, please try again!");

            }
        }

        #endregion
    }
}
