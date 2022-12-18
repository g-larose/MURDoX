using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
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
