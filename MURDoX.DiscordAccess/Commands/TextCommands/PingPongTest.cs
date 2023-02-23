using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace MURDoX.DiscordAccess.Commands.TextCommands
{
    public class PingPongTest : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ITextCommandContext _context;
        // create a command group here to respond to ping and pong
        public PingPongTest(IDiscordRestChannelAPI channelApi, ITextCommandContext context)
        {
            _channelApi = channelApi;
            _context = context;
        }
        
        [Command("ping")]
        [ExcludeFromSlashCommands]
        public async Task<Result> PingAsync(string ping)
        {
            Result<IMessage> result = await _channelApi.CreateMessageAsync(_context.Message.ChannelID.Value, $"string: {ping}");
            return !result.IsSuccess ? Result.FromError(result) : Result.FromSuccess();
        }
    }
}