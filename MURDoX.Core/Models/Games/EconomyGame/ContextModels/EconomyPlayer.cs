using System.ComponentModel.DataAnnotations;

namespace MURDoX.Core.Models.Games.EconomyGame.ContextModels
{
    public class EconomyPlayer
    {
        [Key]
        public Guid Id { get; set; }
        public int Starstone { get; set; }
        public int Chromotite { get; set; }
        public int Zoridium { get; set; }
        public int Money { get; set; }
        public long UserId { get; set; }
        public List<Guid> OwnedPlanetIds { get; set; } = new();
    }
}