using Remora.Discord.API.Abstractions.Objects;

namespace MURDoX.Core.Models.Utility.TicketService;

public class TicketServiceInput
{
    public TicketType Type { get; set; }
    public string? Command { get; set; }
    public string? Content { get; set; }
    public IUser? Author { get; set; }
}