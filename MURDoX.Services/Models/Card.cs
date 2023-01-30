using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class Card
    {
        public string Suit { get; set; }
        public string Value { get; set; }

        public Card(string suit, string value)
        {
            Suit = suit;
            Value = value;
        }
    }
}
