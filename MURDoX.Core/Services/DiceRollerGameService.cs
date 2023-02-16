using System.Diagnostics;
using MURDoX.Core.Data;
using System.Timers;
using Microsoft.EntityFrameworkCore.Query;
using MURDoX.Core.Models.Games;
using Remora.Results;

namespace MURDoX.Core.Services;

public class DiceRollerGameService
{
    private readonly ApplicationDbContext _db;
    private DiceRollerInput _input;
    public DiceRollerGameService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<DiceRollerResponse> DoRollAsync(DiceRollerInput input)
    {
        _input = input;
        DiceRollerResponse response = new DiceRollerResponse();
        var rnd = new Random();
        
        for (int i = 0; i < input.Dice; i++)
        {
            response.DiceResults.Add(new ValueTuple<string, int>(input.Players[0], rnd.Next(1, input.Sides)));
            response.DiceResults.Add(new ValueTuple<string, int>(input.Players[1], rnd.Next(1, input.Sides)));
        }

        response.Dice = input.Dice;
        response.Sides = input.Sides;

        return response;
    }
}