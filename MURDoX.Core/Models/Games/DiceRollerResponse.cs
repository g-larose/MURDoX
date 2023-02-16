namespace MURDoX.Core.Models.Games;

public class DiceRollerResponse
{
    public List<(string, int)>? DiceResults { get; set; }
    public int Sides { get; set; }
    public int Dice { get; set; }
}