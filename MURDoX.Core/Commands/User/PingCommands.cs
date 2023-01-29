using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.User
{
    public class PingCommands : BaseCommandModule
    {
        private readonly AppDbContextFactory _dbFactory;
        private ILoggerService _logger;
        private readonly UtilityHelper _utilityHelper;

        public PingCommands(AppDbContextFactory dbFactory, ILoggerService logger) 
        { 
            _dbFactory = dbFactory;
            _logger = logger;
            _utilityHelper = new UtilityHelper(_dbFactory, _logger);
           ;
        }
        [Command("ping")]
        [Description("ping the database and Discord Gateway for a response time")]
        public async Task Ping(CommandContext ctx)
        {
            var bot = ctx.Client.CurrentUser.Username;
            var embedBuilder = new EmbedBuilderHelper();
            var ping = new Ping();
            var dbLatency = _utilityHelper.GetDbLatency();
            var fields = new EmbedField[]
            {
                new EmbedField { Name = "→ Database Latency ←", Value = "```cs\n" + "Calculating..".PadLeft(12, '⠀') + "```", Inline = true },
                new EmbedField { Name = "→ Discord API Latency ←", Value = "```cs\n" + "Calculating..".PadLeft(12, '⠀') + "```", Inline = true },
                new EmbedField { Name = "→ Web Socket Latency ←", Value = "```cs\n" + "Calculating..".PadLeft(12, '⠀') + "```", Inline = true }
            };

            var embed = new Embed()
            {
                Desc = "Fetching Ping Results...",
                Fields = fields,
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Footer = $"{bot} doing work...  {DateTimeOffset.Now}",
            };

            var message = await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            await Task.Delay(800);

            var sw = Stopwatch.StartNew();

            await ctx.Channel.TriggerTypingAsync();
            sw.Stop();

            var pingTimer = Stopwatch.StartNew();
            PingReply reply = ping.Send("google.com");
            var pingStatus = reply.Status;
            pingTimer.Stop();

            var apiLat = sw.ElapsedMilliseconds.ToString("N0");
            var pingLat = pingTimer.ElapsedMilliseconds.ToString("N0");

            var modifiedFields = new EmbedField[]
            {
                new EmbedField { Name = "→ Database Latency ←", Value = "```cs\n" + $"{dbLatency}".PadLeft(12, '⠀') + "ms ```", Inline = true },
                new EmbedField { Name = "→ Discord API Latency ←", Value = "```cs\n" + $"{apiLat}".PadLeft(12, '⠀') + "ms ```", Inline = true },
                new EmbedField { Name = "→ Web Socket Latency ←", Value = "```cs\n" + $"{pingLat}".PadLeft(12, '⠀') + "ms ```", Inline = true }
            };
            var modifiedEmbed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Fields = modifiedFields,
                TimeStamp = DateTime.Now,
                Footer = $"{bot}"
            };
            await message.ModifyAsync(x => x.WithEmbed(embed: embedBuilder.Build(modifiedEmbed)));

            
        }
    }
}
