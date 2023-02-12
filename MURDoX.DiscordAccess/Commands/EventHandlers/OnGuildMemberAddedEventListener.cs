using DSharpPlus;
using DSharpPlus.EventArgs;
using MURDoX.Core.Helpers;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Utility.WelcomeService;
using MURDoX.Core.Services;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnGuildMemberAddedEventListener
    {
        internal async Task OnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            EmbedBuilderHelper embedBuilder = new();
            WelcomeService welcomeService = new();
            WelcomeServiceInput welcomeServiceInput = new()
            {
                Username = e.Member.Username
            };
            WelcomeServiceResponse response = welcomeService.GetWelcomeMessage(welcomeServiceInput).Result;
            Embed embed = new Embed()
            {
                Title = $"{e.Member.Username} welcome to {e.Guild.Name}",
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = $"{response.Message}"
            };
            await e.Guild.GetDefaultChannel().SendMessageAsync(response.Message);
            await e.Member.SendMessageAsync(embed: embedBuilder.Build(embed));
        }
    }
}