using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnGuildMemberAddedEventListener
    {
        internal Task OnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            ulong userId = e.Member.Id;
            var currentGuild = e.Guild;
            
            return Task.CompletedTask;
        }
    }
}