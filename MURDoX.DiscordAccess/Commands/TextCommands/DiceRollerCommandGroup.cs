﻿using System.ComponentModel;
using System.Text;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Games;
using MURDoX.Core.Services;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace MURDoX.DiscordAccess.Commands.TextCommands;

public class DiceRollerCommandGroup : CommandGroup
{
    private readonly ITextCommandContext _context;
    private readonly IDiscordRestUserAPI _users;
    private readonly IDiscordRestChannelAPI _channels;
    private readonly DiceRollerGameService _diceGameService;

    public DiceRollerCommandGroup(ITextCommandContext context, IDiscordRestUserAPI users, IDiscordRestChannelAPI channels, DiceRollerGameService diceGameService)
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
        DiceRollerResponse response = await _diceGameService.DoRollAsync(new DiceRollerInput { Players = players, Dice = dice, Sides = sides });

        var fields = new EmbedField[6];
        var playerOneDiceResults = response.PlayerOneResults;
        var playerTwoDiceResults = response.PlayerTwoResults;
        var pOneDice = new StringBuilder();
        var pTwoDice = new StringBuilder();
        foreach (KeyValuePair<string, List<int>> d in playerOneDiceResults)
        {
            for (int i = 0; i < d.Value.Count; i++)
            {
                if (i < 5)
                    pOneDice.Append(d.Value[i] + ",");
                else
                    pOneDice.Append(d.Value[i]);

            }
        }
        foreach (KeyValuePair<string, List<int>> d in playerTwoDiceResults)
        {
            for (int i = 0; i < d.Value.Count; i++)
            {
                if (i < 5)
                    pTwoDice.Append(d.Value[i] + ",");
                else
                    pTwoDice.Append(d.Value[i]);

            }
        }

        fields[0] = new("Player", players[0], true);
        fields[1] = new("Results", pOneDice.ToString(), true);
        fields[2] = new("Dice", $"{response.Dice}", true);
        fields[3] = new("Player", players[1], true);
        fields[4] = new("Results", pTwoDice.ToString(), true);
        fields[5] = new("Dice", $"{response.Dice}", true);
        var embed = new Embed()
        {
            Title = $"Results for Game {players[0]} vs {players[1]}",
            Fields = fields
        };
        Result<IMessage> result = await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new[] { embed });
        return !result.IsSuccess ? Result.FromError(result) : Result.FromSuccess();
    }
}
// there we go dice service was at fault as i said