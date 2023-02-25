using Remora.Discord.API.Abstractions.Objects;

namespace MURDoX.Core.Models.Utility.TicketService;

public class Ticket
{
    public Guid Id { get; set; }
    public TicketType Type { get; set; }
    public IUser Author { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Created_Timestamp { get; set; }
}