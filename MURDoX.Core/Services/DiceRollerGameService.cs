#region

using System.Text;
using MURDoX.Core.Models.Games;

#endregion

namespace MURDoX.Core.Services
{
    public class DiceRollerGameService
    {
        public async Task<DiceRollerResponse> DoRollAsync(DiceRollerInput input)
        {
            DiceRollerResponse? response = new();
            try
            {
                response = new DiceRollerResponse();
                Random rnd1 = new();
                Random rnd2 = new();
                StringBuilder? playerOneBuilder = new();
                StringBuilder? playerTwoBuilder = new();
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

                List<int>? pOneResults = playerOneBuilder.ToString().Trim()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                List<int>? pTwoResults = playerTwoBuilder.ToString().Trim()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                response.PlayerOneResults.Add(input.Players[0], pOneResults);
                response.PlayerTwoResults.Add(input.Players[1], pTwoResults);
                response.Dice = input.Dice;
                response.Sides = input.Sides;
                int pOneScore = pOneResults.Sum();
                int pTwoScore = pTwoResults.Sum();

                if (pOneScore > pTwoScore)
                {
                    response.Winner = (input.Players[0], pOneScore);
                }
                else if (pTwoScore > pOneScore)
                {
                    response.Winner = (input.Players[1], pTwoScore);
                }
                else
                {
                    response.Winner = ("tie", pOneScore);
                }

                //TODO: save the results to the db.
            }
            catch (Exception ex)
            {
                string? test = ex.Message;
            }

            return response;
        }
    }
}