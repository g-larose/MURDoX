namespace MURDoX.Core.Models.Games;

public class DiceRollerResponse
{
    public List<int>? DiceResults { get; set; }
    public List<string>? Players { get; set; }
    public int Sides { get; set; }
    public int Dice { get; set; }
}