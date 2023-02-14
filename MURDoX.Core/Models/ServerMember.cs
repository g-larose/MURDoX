using System.ComponentModel.DataAnnotations;
using MURDoX.Core.Enums;

namespace MURDoX.Core.Models
{
    public class ServerMember
    {
        [Key]
        public int Id { get; set; }
        public ulong DiscordId { get; set; }
        public string Username { get; set; } = "";
        public Rank Rank { get; set; }
        public string AvatarUrl { get; set; } = "";
        public int Warnings { get; set; }
        public int Thanks { get; set; }
        public int Xp { get; set; }
        public int BankAccountTotal { get; set; }
        public DateTime Created { get; set; }
    }
}
