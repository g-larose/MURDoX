namespace MURDoX.Core.Models.Games;

public class DiceRollerResponse
{
    public Dictionary<string, List<int>>? PlayerOneResults { get; set; } = new();
    public Dictionary<string, List<int>>? PlayerTwoResults { get; set; } = new();
    public int Sides { get; set; }
    public int Dice { get; set; }
    public ValueTuple<string, int> Winner { get; set; }
}