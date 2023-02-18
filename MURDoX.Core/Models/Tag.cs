#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace MURDoX.Core.Models
{
    public class Tag
    {
        [Key] public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
        public string TagDesc { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
    }
}