using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Utilities.Converters
{
    public class StringToCategoryConverter 
    {
        public static string StringToCatConverter(string cat)
        {
            if (cat is null or "")
                return "9";

            string result = cat switch
            {
                "general knowledge" => "9",
                "books" => "10",
                "films" => "11",
                "games" => "16",
                "science" => "17",
                "computers" => "18",
                "mythology" => "20",
                "sports" => "21",
                "geography" => "22",
                "history" => "23",
                "politics" => "24",
                "art" => "25",
                "celebrities" => "26",
                "animals" => "27",
                _ => "9"
            };
            return result;
        }
    }
}
