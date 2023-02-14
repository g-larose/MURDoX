namespace MURDoX.Core.Models.Utility.SuggestionService
{
    public class Suggestion
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Discription { get; set; }
        public ulong AuthorId { get; set; }
        public DateTime Created { get; set; }
    }
}
