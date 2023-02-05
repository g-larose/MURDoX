using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Bot
{
    public class BotInfoCommands : BaseCommandModule
    {
        
        [Command("botinfo")]
        [Description("get bot info")]
        public async Task BotInfo(CommandContext ctx)
        {
            var bot = ctx.Client.CurrentUser;
            var heapMemory = $"{GC.GetTotalMemory(true) / 1024 / 1024:n0} MB";
           // var memberCount = ctx.Guild.MemberCount;
            int guildCount = ctx.Client.Guilds.Count;
            var uptime = TimerService.GetBotUpTime();
            var fields = new EmbedField[4];
            //Desc = $"{botName} has been online for **{uptime}**"
           

            fields[0] = new EmbedField { Name = "Bot Name:", Value = $"{bot.Username} ", Inline = true };
            fields[1] = new EmbedField { Name = "Memory:", Value = $"{heapMemory} ", Inline = true };
            fields[2] = new EmbedField { Name = "Guilds:", Value = $"{guildCount} ", Inline = true };
            fields[3] = new EmbedField { Name = "Uptime", Value = $"{bot.Username} has been online for **{uptime}**", Inline = true };

            var embedBuilder = new EmbedBuilderHelper();

            var embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                ThumbnailImgUrl = "https://i.imgur.com/tbrKXgH.png",
                Title = "Bot Info", 
                Fields =  fields,
                Footer = $"{bot.Username} {DateTime.Now}",
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
        }
    }
}
