using DSharpPlus;
using DSharpPlus.EventArgs;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnGuildMemberAddedEventListener
    {
        internal Task OnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}