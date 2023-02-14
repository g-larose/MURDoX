using System.Drawing;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace MURDoX.DiscordAccess.Commands.EventHandlers
{
    public class PingPong : IResponder<IMessageCreate>
    {
        private readonly IDiscordRestChannelAPI _channelApi;

        public PingPong(IDiscordRestChannelAPI channelApi)
        {
            _channelApi = channelApi;
        }

        public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
        {
            if (gatewayEvent.Content != "ping")
            {
                return Result.FromSuccess();
            }

            Embed embed = new(Description: "Pong!", Colour: Color.LawnGreen);
            return (Result)await _channelApi.CreateMessageAsync(gatewayEvent.ChannelID, embeds: new[] { embed }, ct: ct);
        }
    }
}