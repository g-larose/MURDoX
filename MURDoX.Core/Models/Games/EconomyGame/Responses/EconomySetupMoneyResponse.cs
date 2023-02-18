namespace MURDoX.Core.Models.Games.EconomyGame.Responses
{
    public class EconomySetupMoneyResponse
    {
        public string Message { get; set; } = string.Empty;
        public int MoneyBefore { get; set; }
        public int MoneyAfter { get; set; }
    }
}