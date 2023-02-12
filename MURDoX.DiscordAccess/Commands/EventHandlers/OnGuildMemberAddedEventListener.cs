using DSharpPlus;
using DSharpPlus.EventArgs;
using MURDoX.Core.Models.Utility.WelcomeService;
using MURDoX.Core.Services;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnGuildMemberAddedEventListener
    {
        internal Task OnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            WelcomeService welcomeService = new();
            WelcomeServiceInput welcomeServiceInput = new()
            {
                Username = e.Member.Username
            };
            WelcomeServiceResponse response = welcomeService.GetWelcomeMessage(welcomeServiceInput).Result;
            // TODO: Send the message to the user or to the welcome channel of the server

            e.Guild.GetDefaultChannel().SendMessageAsync(response.Message);

            return Task.CompletedTask;
        }
    }
}