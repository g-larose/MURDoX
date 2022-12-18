using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Polls
{
    public class PollCommands : ApplicationCommandModule
    {
        #region POLL

        [SlashCommand("poll", "Creates a Poll Slash command")]
        public async Task Poll(InteractionContext ctx, 
            [Option("subject", "question the poll ask's the members")] string subject, 
            [Option("duration", "duration to run the poll")] string duration)
        {
            var embedBuilder = new EmbedBuilderHelper();
            var fields = new EmbedField[2];
            fields[0] = new EmbedField() { Name = "Subject", Value = subject, Inline = true };
            fields[1] = new EmbedField() { Name = "Duration", Value = $"{duration}(seconds)", Inline = true };

            var embed = new Embed()
            {
                Author = ctx.User.Username,
                Desc = $"new poll, this poll will last {duration}(seconds) , react to vote.",
                Fields = fields,
                Footer = $"MURDoX {DateTimeOffset.Now.UtcDateTime}"
            };
            
            var interactivity = ctx.Client.GetInteractivity();
            var msg =  await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            var emojiOptions = new string[] { ":thumbsup:", ":thumbsdown:" };
            
            foreach (var emoji in emojiOptions)
            {
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, emoji));
                await Task.Delay(200);
            }
            var _ = int.TryParse(duration, out int result);
            var emojiResult = await interactivity.CollectReactionsAsync(msg, new TimeSpan(0, 0, result));
            
            var results = emojiResult.Select(x => x.Emoji);
            var thumbsUpCount = 0;
            var thumbsDownCount = 0;

            foreach (var e in results)
            {
                if (e.Name.Equals("👍"))
                    thumbsUpCount++;
                else if (e.Name.Equals("👎"))
                    thumbsDownCount++;
            }
            var resultWinner = string.Empty;
            if (thumbsUpCount > thumbsDownCount)
                resultWinner = "👍";
            else if (thumbsDownCount > thumbsUpCount)
                resultWinner = "👎";
            else
                resultWinner = "Tie";

            var resultFields = new EmbedField[1];

            resultFields[0] = new EmbedField() {  Name = "Winner", Value = resultWinner, Inline= true };
            var modifiedEmbed = new Embed()
            {
                Desc = $"voting results for\r\n``{subject}``",
                Fields =  resultFields,
            };

            await msg.ModifyAsync(x => x.WithEmbed(embed: embedBuilder.Build(modifiedEmbed))); 

        }

        #endregion
    }
}
