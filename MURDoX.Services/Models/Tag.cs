using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }   = string.Empty;
        public string TagName { get; set; }    = string.Empty;
        public string TagDesc { get; set; }    = string.Empty;
        public string Content { get; set; }    = string.Empty;
        public string CreatedBy { get; set; }  = string.Empty;
        public DateTime CreateAt { get; set; }
    }
}
