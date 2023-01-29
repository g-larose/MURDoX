using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Games.BlackJack
{
    public class BlackJackSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("blackjack", "starts a new blackjack game player vs bot")]
        public async Task BlackJack(InteractionContext ctx)
        {
            //TODO: impliment blackjack game
            
            var interactionResponseBuilder = new DiscordInteractionResponseBuilder();
            var followUpBuilder = new DiscordFollowupMessageBuilder();

          
            var embedBuilder = new EmbedBuilderHelper();
            var embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = "Blackjack is in progress...stay tuned for updates."
            };

            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: interactionResponseBuilder.WithContent("Something new"));
           
            await Task.Delay(2000);
            await ctx.FollowUpAsync(builder: followUpBuilder.WithContent("**-> UPDATE <-**"));
            await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));

           
        }
    }
}
