namespace MURDoX.Core.Models.Games;

public class DiceRollerInput
{
    private List<DiscordMember>? Players { get; set; }
    public TimeSpan Duration { get; set; }
    public int Dice { get; set; }
    public int Sides { get; set; }
    
}