using System.Diagnostics;
using System.Text;
using MURDoX.Core.Data;
using System.Timers;
using Microsoft.EntityFrameworkCore.Query;
using MURDoX.Core.Models.Games;
using Remora.Results;

namespace MURDoX.Core.Services;

public class DiceRollerGameService
{
    public async Task<DiceRollerResponse> DoRollAsync(DiceRollerInput input)
    {
        var response = new DiceRollerResponse();
        try
        {
            response = new();
            Random rnd1 = new();
            Random rnd2 = new();
            var playerOneBuilder = new StringBuilder();
            var playerTwoBuilder = new StringBuilder();
            input.Players ??= new List<string>
            {
                "Player 1",
                "Player 2"
            };
            for (int i = 0; i < input.Dice; i++)
            {
                playerOneBuilder.Append(rnd1.Next(1, input.Sides) + " ");
                playerTwoBuilder.Append(rnd2.Next(1, input.Sides) + " ");
            }

            var pOneResults = playerOneBuilder.ToString().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var pTwoResults = playerTwoBuilder.ToString().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            response.PlayerOneResults.Add(input.Players[0], pOneResults);
            response.PlayerTwoResults.Add(input.Players[1], pTwoResults);
            response.Dice = input.Dice;
            response.Sides = input.Sides;
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        
        return response;
    }
}