using System.ComponentModel.DataAnnotations;

namespace MURDoX.Core.Models.Games.EconomyGame.ContextModels
{
    public class EconomySettings
    {
        public int MoneySet { get; set; }
        public int StarstoneSet { get; set; }
        public int ChromotiteSet { get; set; }
        public int ZoridiumSet { get; set; }
        public int DefaultStorageCapacity { get; set; }
        public float ShopMultiplier { get; set; }

        // ServerId is the Discord server ID
        [Key]
        public long ServerId { get; set; }
        // NewsChannelId is the Discord channel ID for the news channel of the game
        public long NewsChannelId { get; set; }
    }
}