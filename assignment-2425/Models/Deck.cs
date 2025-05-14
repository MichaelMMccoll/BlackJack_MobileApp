using assignment_2425.Models;

namespace assignment_2425.ViewModels
{
    public class Deck
    {
        public List<Cards> Cards { get; set; }
        //Initializes the deck
        public Deck()
        {
            Cards = new List<Cards>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    Cards.Add(Player.GetImage(new Cards { Suit = suit, Value = value }));
                }
            }
            Shuffle(Cards);
        }

        private Random rng = new Random();

        //Shuffles the card randomly
        public void Shuffle(List<Cards> list)
        {
            int n = list.Count;

            while (n > 1)
            {
	            n--;
	            int k = rng.Next(n + 1);
	            var value = list[k];
	            list[k] = list[n];
	            list[n] = value;
            }
        }
        }
}
