using System.ComponentModel.DataAnnotations;

namespace MURDoX.Core.Models.Games.EconomyGame.ContextModels
{
    public class EconomyPlanet
    {
        [Key]
        public Guid Id { get; set; }
        public int StarstoneGeneratorAmount { get; set; }
        public int ChromotiteGeneratorAmount { get; set; }
        public int ZoridiumGeneratorAmount { get; set; }
        public int MoneyGeneratorAmount { get; set; }
        public int StorageCapacity { get; set; }
    }
}