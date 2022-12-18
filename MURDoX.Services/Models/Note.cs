using DSharpPlus.Entities;
using MURDoX.Services.Enum;
using System.ComponentModel.DataAnnotations;

namespace MURDoX.Services.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public Guid NoteGuid { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
        public DiscordUser? Author { get; set; }
        public DateTimeOffset Created { get; set; }
        public NoteStatus Status { get; set; }


    }
}
