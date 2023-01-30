using MURDoX.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class Deck
    {
        private static string[] Suits = { "Hearts", "Diamonds", "Spades", "Clubs" };
        private static string[] Values = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        private List<Card> Cards;

        public Deck()
        {
            Cards  = new List<Card>();

            foreach (var suit in Suits) 
            {
                foreach (var value in Values)
                {
                    Cards.Add(new Card(suit, value));
                }
            }
        }

        public Card Deal()
        {
            var cards = Cards.ToArray();
            cards.Shuffle();
            Card topCard = cards[0];
            Cards.RemoveAt(0);
            return topCard;
        }
    }
}
