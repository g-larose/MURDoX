using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.User
{
    public class PingCommands : BaseCommandModule
    {
        private readonly AppDbContextFactory _dbFactory;
        private readonly UtilityHelper utilHelper;
        public PingCommands(AppDbContextFactory dbFactory) 
        { 
            _dbFactory = dbFactory;
            utilHelper = new UtilityHelper(_dbFactory);
        }
        [Command("ping")]
        [Description("ping the database and Discord Gateway for a response time")]
        public async Task Ping(CommandContext ctx)
        {
            var bot = ctx.Client.CurrentUser.Username;
            var embedBuilder = new EmbedBuilderHelper();
            var dbLatency = utilHelper.GetDbLatency();
            var fields = new EmbedField[]
            {
                new EmbedField { Name = "→ Database Latency ←", Value = "```cs\n" + "Fetching..".PadLeft(15, '⠀') + "```", Inline = true },
                new EmbedField { Name = "→ Discord API Latency ←", Value = "```cs\n" + "Fetching..".PadLeft(15, '⠀') + "```", Inline = true }
            };

            var embed = new Embed()
            {
                Desc = "Fetching Ping Results...",
                Fields = fields,
                Footer = $"{bot} doing work...  {DateTimeOffset.Now}",
            };

            var message = await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            await Task.Delay(800);

            var sw = Stopwatch.StartNew();

            await ctx.Channel.TriggerTypingAsync();

            sw.Stop();

            var apiLat = sw.ElapsedMilliseconds.ToString("N0");

            var modifiedFields = new EmbedField[]
            {
                new EmbedField { Name = "→ Database Latency ←", Value = "```cs\n" + $"{dbLatency}".PadLeft(15, '⠀') + "ms ```", Inline = true },
                new EmbedField { Name = "→ Discord API Latency ←", Value = "```cs\n" + $"{apiLat}".PadLeft(15, '⠀') + "ms ```", Inline = true }
            };
            var modifiedEmbed = new Embed()
            {
                Fields = modifiedFields,
            };
            await message.ModifyAsync(x => x.WithEmbed(embed: embedBuilder.Build(modifiedEmbed)));

            
        }
    }
}
