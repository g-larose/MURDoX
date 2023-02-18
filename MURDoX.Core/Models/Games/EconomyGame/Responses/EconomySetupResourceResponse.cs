namespace MURDoX.Core.Models.Games.EconomyGame.Responses
{
    public class EconomySetupResourceResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StarstoneBefore { get; set; }
        public int StarstoneAfter { get; set; }
        public int ChromotiteBefore { get; set; }
        public int ChromotiteAfter { get; set; }
        public int ZoridiumBefore { get; set; }
        public int ZoridiumAfter { get; set; }
    }
}