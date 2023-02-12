using DSharpPlus;
using DSharpPlus.EventArgs;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    internal class OnMessageCreatedEventListener
    {
        internal Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}