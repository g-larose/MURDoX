namespace MURDoX.Core.Models.Games.EconomyGame.Responses
{
    public class EconomySetupShopResponse
    {
        public string Message { get; set; } = string.Empty;
        public float ShopEffectMultiplierBefore { get; set; }
        public float ShopEffectMultiplierAfter { get; set; }
    }
}