using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Humanizer;
using Humanizer.Localisation;
using MURDoX.Core.Extensions;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Reminders
{
    public class ReminderSlashCommand : ApplicationCommandModule
    {
      
        [SlashCommand("remind", "creates a reminder")]
        public async Task Reminder(InteractionContext ctx,
             [Option("duration", "I can understand 2m, 5m or one day or in five minutes")] string duration,
             [Option("reminder", "the reminder")] string reminderContent)
        {
            
            TimeSpan _minimumReminderTime = TimeSpan.FromMinutes(3);
            var timerServiceHelper = new TimerServiceHelper(ctx.Client);
            var offset = await timerServiceHelper.GetOffsetForUserAsync(ctx.User.Id);
            var timeResult = TimerServiceHelper.ExtractTime(duration, offset, out _);
            var bot = ctx.Client.CurrentUser;
            var durationFormatted = TimerServiceHelper.TryParse(duration);
            var responseBuilder = new DiscordInteractionResponseBuilder();
            var followupBuilder = new DiscordFollowupMessageBuilder();
            Embed embed;
            var embedBuilder = new EmbedBuilderHelper();
            var msgResponse = ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: responseBuilder.WithContent("MURDoX is trying to extract the duration"));
            await ctx.Channel.TriggerTypingAsync();
            await Task.Delay(2000);

            TimeSpan parsedTime;
            if (!timeResult.IsDefined(out parsedTime))
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = timeResult.Error!.Message,
                };

                await ctx.FollowUpAsync(builder: followupBuilder.AddEmbed(embed: embedBuilder.Build(embed)));
                return;
            }
               
            if (parsedTime <= TimeSpan.Zero)
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "You can't set a reminder in the past!",
                };
                await ctx.FollowUpAsync(builder: followupBuilder.AddEmbed(embed: embedBuilder.Build(embed)));
                return;
            }
              

            if (parsedTime < _minimumReminderTime)
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"You can't set a reminder less than {_minimumReminderTime.Humanize(minUnit: TimeUnit.Minute)}!",
                };
                await ctx.FollowUpAsync(builder: followupBuilder.AddEmbed(embed: embedBuilder.Build(embed)));
                return;
            }
                
               
            if (!durationFormatted.IsSuccess)
            {
                await ctx.Channel.SendMessageAsync("failed to extract time from input!");
                return;
            }

            var reminderTime = DateTimeOffset.UtcNow + parsedTime;
            var totalDuration = durationFormatted.Entity.Humanize(1, maxUnit: TimeUnit.Year, minUnit: TimeUnit.Minute);
            
            embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = $"Ok , I will remind you ``{reminderContent}`` in ``{totalDuration}`` from now"
            };
            ReminderService reminderService = new(ctx.Channel, ctx.Member, timeResult, reminderContent);
            await ctx.FollowUpAsync(builder: followupBuilder.AddEmbed(embed: embedBuilder.Build(embed)));


                
        }

     
    }
}
