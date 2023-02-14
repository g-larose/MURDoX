using MURDoX.Core.Data;
using MURDoX.Core.Models.Utility.SuggestionService;

namespace MURDoX.Core.Services
{
    public class SuggestionService
    {
        private readonly ApplicationDbContext _db;

        public SuggestionService(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<SuggestionServiceResponse> AddSuggestionAsync(SuggestionServiceInput suggestionServiceInput)
        {
            Suggestion suggestion = new()
            {
                Id = Guid.NewGuid(),
                Name = suggestionServiceInput.Name,
                Discription = suggestionServiceInput.Description,
                AuthorId = suggestionServiceInput.AuthorId,
                Created = DateTime.Now
            };

            if (_db.Suggestions == null)
            {
                return new SuggestionServiceResponse
                {
                    Id = Guid.Empty,
                    Name = "Error",
                    Description = "Error"
                };
            }

            await _db.Suggestions.AddAsync(suggestion);
            await _db.SaveChangesAsync();
            return new SuggestionServiceResponse
            {
                Id = suggestion.Id,
                Name = suggestion.Name,
                Description = suggestion.Discription
            };

        }
    }
}