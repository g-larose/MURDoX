using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Emzi0767.Utilities;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Suggestions
{
   
    public class SuggestionSlashCommand : ApplicationCommandModule
    {
        //private readonly AppDbContextFactory _dbFactory;

        //public SuggestionSlashCommand(AppDbContextFactory dbFactory)
        //{
        //    _dbFactory = dbFactory;
        //}

        [SlashCommand("suggestion", "suggestion slash command")]
        public async Task SuggestionCommand(InteractionContext ctx,
            [Option("title", "the title for the suggestion")] string title,
            [Option("content", "the suggsetion content")] string content)
        {
            try
            {
                var guild = ctx.Guild.Name; 
                ulong channelId = 0;

                switch (guild)
                {
                    case "DroppsDev":
                        channelId = 1065545582615216179;
                        break;
                    case "Bot Playground":
                        channelId = 764184469380661289;
                        break;

                    default:
                        channelId = 795412870438453321;
                        break;
                }

                var suggestion = new Suggestion()
                {
                    Name = title,
                    Discription = content,
                    AuthorId = ctx.Member.Id,
                    Created = DateTime.UtcNow,
                };

                  
                var embedBuilder = new EmbedBuilderHelper();
                var updateEmbed = new Embed();
                var fields = new EmbedField[]
                {
                    new EmbedField { Name = "Suggestion Name", Value = $"{title}" , Inline = true },
                    new EmbedField { Name = "Suggestion Content", Value = $"{content}" , Inline = true },
                };

                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Title = title,
                    Desc = content,
                    Fields = fields,
                    TimeStamp = DateTime.Now,
                    Footer = "MURDoX"

                };

                var msg = await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                updateEmbed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "Saving Suggestion"
                };

                var followUpBuilder = new DiscordFollowupMessageBuilder();
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource, builder: new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder.Build(updateEmbed)));
                await Task.Delay(2000);

                var followUpMessage = await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"MURDoX is Saving {suggestion.Name}"));
                await Task.Delay(3000);

                await ctx.DeleteFollowupAsync(followUpMessage.Id);
                UtilityHelper.SaveSuggestionToDb(suggestion);
                Task.Delay(3000).Wait();

                //await msg.DeleteAsync();
                var memberName = await ctx.Guild.GetMemberAsync(suggestion.AuthorId);
                var channel = ctx.Guild.GetChannel(channelId);

                updateEmbed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"suggestion ``[{suggestion.Name}]`` saved to database with Id {suggestion.Id} created by: ``[{memberName.DisplayName}]``",
                };

                await msg.DeleteAsync();
                await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"MURDoX Saved ``[{suggestion.Name}]`` Successfully"));
                await channel.SendMessageAsync(embed: embedBuilder.Build(updateEmbed));
            }
            catch (Exception ex)
            {
                var m = ex.Message; 
            }

            
        }
    }
}
