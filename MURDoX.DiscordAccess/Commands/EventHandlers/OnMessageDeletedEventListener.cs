using DSharpPlus;
using DSharpPlus.EventArgs;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnMessageDeletedEventListener
    {
        internal Task OnMessageDeleted(DiscordClient sender, MessageDeleteEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}