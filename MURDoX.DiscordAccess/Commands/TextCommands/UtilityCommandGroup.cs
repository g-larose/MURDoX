using System.Drawing;
using MURDoX.Core.Models.Utility.SuggestionService;
using MURDoX.Core.Services;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Rest.Core;
using Remora.Results;

namespace MURDoX.DiscordAccess.Commands.TextCommands
{
    public class UtilityCommandGroup : CommandGroup
    {
        private readonly ITextCommandContext _context;
        private readonly IDiscordRestChannelAPI _channels;
        private readonly SuggestionService _suggestionService;

        public UtilityCommandGroup(ITextCommandContext context, IDiscordRestChannelAPI channels, SuggestionService suggestionService)
        {
            _context = context;
            _channels = channels;
            _suggestionService = suggestionService;
        }
        
        [Command("suggest")]
        public async Task<Result> SuggestAsync(string name, string description)
        {
            SuggestionServiceResponse response =
                await _suggestionService.AddSuggestionAsync(new SuggestionServiceInput
                {
                    AuthorId = _context.Message.Author.Value.ID.Value,
                    Name = name,
                    Description = description
                });
            if (response is { Description: { }, Name: { } })
            {
                Embed embed = new()
                {
                    Title = "Suggestion",
                    Colour = Color.Green,
                    Fields = new Optional<IReadOnlyList<IEmbedField>>(new List<IEmbedField>()
                    {
                        new EmbedField("Name", response.Name, true),
                        new EmbedField("Description", response.Description, true),
                        new EmbedField("Under ID", response.Id.ToString(), true)
                    })
                };
                List<Embed> embeds = new()
                {
                    embed
                };
                Result<IMessage> result = await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: embeds);
                return !result.IsSuccess ? Result.FromError(result) : Result.FromSuccess();
            }
            else
            {
                Embed embed = new()
                {
                    Title = "Suggestion",
                    Colour = Color.Red,
                    Description = "Error"
                };
                List<Embed> embeds = new()
                {
                    embed
                };
                Result<IMessage> result = await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: embeds);
                return !result.IsSuccess ? Result.FromError(result) : Result.FromSuccess();
            }
        }
    }
}