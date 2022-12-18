using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Core.Utilities.Converters;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Facts
{
    public class FactCommands : BaseCommandModule
    {
        System.Timers.Timer _timer;
        CommandContext _ctx;
        int seconds = 0;
        [Command("fact")]
        [RequirePermissions(DSharpPlus.Permissions.ManageChannels)]
        public async Task Start(CommandContext ctx, [RemainingText] string args)
        {
            if (args != null || args != "")
            {
                
                switch (args)
                {
                    case "start":
                        _ctx = ctx;
                        _timer = new System.Timers.Timer();
                        _timer.Interval = 1000;
                        _timer.Start();
                        _timer.Elapsed += _timer_Elapsed;
                        var embed = new Embed()
                        {
                            Author = "Facts will begin shortly",
                            Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                            TimeStamp = null,
                        };

                        var embedBuilder = new EmbedBuilderHelper();
                        await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        break;
                    case "stop":
                        _timer.Stop();
                        var messageAuthor = ctx.Message.Author;
                        embed = new Embed()
                        {
                            Author = $"Facts have been stopped, by {messageAuthor.Username}",
                            Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                            TimeStamp = null,
                        };

                        embedBuilder = new EmbedBuilderHelper();
                        await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        break;
                    default:
                        break;
                }
            }
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            seconds++;
            var milliseconds = seconds * 1000;
            var hours = MillisecondConverter.ConvertMillisecondsToHours(milliseconds);

            if (milliseconds >= 60000)
            {
                Task.Run(async () =>
               {
                   var bot = _ctx.Client.CurrentUser;
                   var fact = await FactHelper.GetRandomFact();
                   var embedBuilder = new EmbedBuilderHelper();
                   var embed = new Embed()
                   {
                       Title = "Fact",
                       Author = $"{bot.Username} ",
                       Desc = $"{fact}",
                       Footer = $"{bot.Username}©️",
                       AuthorAvatar = bot.AvatarUrl,
                       LinkUrl = "",
                       ThumbnailImgUrl = null,
                       TimeStamp = DateTimeOffset.Now,
                       FooterImgUrl = bot.AvatarUrl,
                       Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                   };
                   await _ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
                   seconds = 0;
               });
            }
           
            
        }
    }
}
