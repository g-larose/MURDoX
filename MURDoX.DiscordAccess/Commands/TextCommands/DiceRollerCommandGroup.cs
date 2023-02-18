#region

using System.ComponentModel;
using System.Text;
using MURDoX.Core.Models.Games;
using MURDoX.Core.Services;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

#endregion

namespace MURDoX.DiscordAccess.Commands.TextCommands
{
    public class DiceRollerCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channels;
        private readonly ITextCommandContext _context;
        private readonly DiceRollerGameService _diceGameService;
        private readonly IDiscordRestUserAPI _users;

        public DiceRollerCommandGroup(ITextCommandContext context, IDiscordRestUserAPI users,
            IDiscordRestChannelAPI channels, DiceRollerGameService diceGameService)
        {
            _context = context;
            _users = users;
            _channels = channels;
            _diceGameService = diceGameService;
        }

        [Command("dicestart")]
        [Description("starts a new dice roller game")]
        public async Task<Result> DiceRollAsync(string user, int dice, int sides)
        {
            List<string> players = new()
            {
                _context.Message.Author.Value.Username,
                user
            };
            DiceRollerResponse response = await _diceGameService.DoRollAsync(new DiceRollerInput
                { Players = players, Dice = dice, Sides = sides });

            EmbedField[]? fields = new EmbedField[8];
            Dictionary<string, List<int>>? playerOneDiceResults = response.PlayerOneResults;
            Dictionary<string, List<int>>? playerTwoDiceResults = response.PlayerTwoResults;
            StringBuilder? pOneDice = new();
            StringBuilder? pTwoDice = new();
            foreach (KeyValuePair<string, List<int>> d in playerOneDiceResults)
                for (int i = 0; i < d.Value.Count; i++)
                    if (i < 5)
                    {
                        pOneDice.Append(d.Value[i] + ",");
                    }
                    else
                    {
                        pOneDice.Append(d.Value[i]);
                    }

            foreach (KeyValuePair<string, List<int>> d in playerTwoDiceResults)
                for (int i = 0; i < d.Value.Count; i++)
                    if (i < 5)
                    {
                        pTwoDice.Append(d.Value[i] + ",");
                    }
                    else
                    {
                        pTwoDice.Append(d.Value[i]);
                    }

            fields[0] = new EmbedField("Player", players[0], true);
            fields[1] = new EmbedField("Results", pOneDice.ToString(), true);
            fields[2] = new EmbedField("Dice", $"{response.Dice}", true);
            fields[3] = new EmbedField("Player", players[1], true);
            fields[4] = new EmbedField("Results", pTwoDice.ToString(), true);
            fields[5] = new EmbedField("Dice", $"{response.Dice}", true);
            fields[6] = new EmbedField("Winner", $"{response.Winner.Item1}", true);
            fields[7] = new EmbedField("Score", $"{response.Winner.Item2}", true);
            Embed? embed = new()
            {
                Title = $"Results for Game {players[0]} vs {players[1]}",
                Fields = fields,
                Timestamp = DateTimeOffset.UtcNow
            };
            Result<IMessage> result =
                await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new[] { embed });
            return !result.IsSuccess ? Result.FromError(result) : Result.FromSuccess();
        }
    }
}
// there we go dice service was at fault as i said