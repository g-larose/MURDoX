namespace MURDoX.Core.Models.Games.EconomyGame.Responses
{
    public class EconomyNewPlayerResponse
    {
        public string Message { get; set; } = string.Empty;
        public int Money { get; set; }
        public int Starstone { get; set; }
        public int Chromotite { get; set; }
        public int Zoridium { get; set; }
        public string PlanetName { get; set; } = string.Empty;
        public Guid PlanetId { get; set; }
    }
}