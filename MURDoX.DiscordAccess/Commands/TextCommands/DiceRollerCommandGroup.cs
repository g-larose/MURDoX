using System.ComponentModel;
using MURDoX.Core.Models;
using MURDoX.Core.Models.Games;
using MURDoX.Core.Services;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
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
    public async Task<Result> DiceRollAsync(string user, string dice, string sides)
    {
        if (int.TryParse(dice, out var _dice));
        if (int.TryParse(sides, out int _sides)) ;
        var players = new List<string>();
        players.Add(_context.Message.Author.Value.Username);
        players.Add(user);
        DiceRollerResponse response = await _diceGameService.DoRoll(new DiceRollerInput()
        {
            Players = players,
            Dice = _dice,
            Sides = _sides,
        });
        return Result.FromSuccess();//this is just to get rid of the error.
    }
}