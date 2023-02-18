namespace MURDoX.Core.Models
{
    public class Reminder
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}