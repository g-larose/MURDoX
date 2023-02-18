#region

using MURDoX.Core.Models.Utility.WelcomeService;
using MURDoX.Core.Services;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Rest.Core;
using Remora.Results;

#endregion

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class OnGuildMemberAddedEventListener : IResponder<IGuildMemberAdd>
    {
        private readonly IDiscordRestChannelAPI _discordRestChannelApi;
        private readonly IDiscordRestUserAPI _discordRestUserApi;
        private readonly WelcomeService _welcomeService;

        public OnGuildMemberAddedEventListener(WelcomeService welcomeService,
            IDiscordRestChannelAPI discordRestChannelApi, IDiscordRestUserAPI discordRestUserApi)
        {
            _welcomeService = welcomeService;
            _discordRestChannelApi = discordRestChannelApi;
            _discordRestUserApi = discordRestUserApi;
        }

        public async Task<Result> RespondAsync(IGuildMemberAdd gatewayEvent, CancellationToken ct = new())
        {
            WelcomeServiceResponse welcomeServiceResponse = await _welcomeService.GetWelcomeMessage(
                new WelcomeServiceInput
                {
                    Username = gatewayEvent.User.Value.Username
                });

            Result<IChannel> result =
                await _discordRestUserApi.CreateDMAsync(new Snowflake(gatewayEvent.User.Value.ID.Value), ct);

            if (!result.IsSuccess)
            {
                return Result.FromError(result);
            }

            string message = welcomeServiceResponse.Message ?? throw new InvalidOperationException("Message is null");
            Result<IMessage> messageResult =
                await _discordRestChannelApi.CreateMessageAsync(result.Entity.ID, message, ct: ct);

            return !messageResult.IsSuccess ? Result.FromError(messageResult) : Result.FromSuccess();
        }
    }
}