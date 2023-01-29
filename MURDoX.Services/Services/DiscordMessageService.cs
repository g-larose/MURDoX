using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class DiscordMessageService
    {
        public async Task HandleDeletedMessage(DiscordChannel channel, DiscordMessage msg)
        {
            ulong DroppsDevLogChannelId = 1004370669649281034;
            ulong BotPlaygroundLogChannelId = 888659367824601160;
            var guild = channel.Guild;
            var member = msg.Author;
            ulong channelId = guild.Name switch
            {
                "DroppsDev" => DroppsDevLogChannelId,
                "Bot Playground" => BotPlaygroundLogChannelId,
                _ => BotPlaygroundLogChannelId,
            };

            var logChannel = guild.GetChannel(channelId);
            await logChannel.SendMessageAsync($"{member.Username} deleted a message from {channel.Name}");

        }
    }
}
