namespace MURDoX.Core.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Discription { get; set; }
        public ulong AuthorId { get; set; }
        public DateTime Created { get; set; }
    }
}
