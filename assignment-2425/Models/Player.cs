using assignment_2425.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;

namespace assignment_2425.ViewModels
{
    public partial class Player : ObservableObject
    {
        [ObservableProperty]
        public int balance = 2000;
        public ObservableCollection<Cards> CurrentHand { get; set; } = new();
        [ObservableProperty]
        public string username = "";
        [ObservableProperty]
        public int wins = 0;
        [ObservableProperty]
        public int losses = 0;
        [ObservableProperty]
        public int currenthandvalue;
        [ObservableProperty]
        public int bet;
        [ObservableProperty]
        public string password = "";
        [ObservableProperty]
        public string idtoken = "";
        [ObservableProperty]
        public string userid = "";
        [ObservableProperty]
        public string imageurl = "defaultprofile.svg";
        [ObservableProperty]
        public string email = "";
        [ObservableProperty]
        public string deck = "";

        public string GetBetImage()
        {
            return Bet switch
            {
                10 => "ten_chip.svg",
                25 => "twentyfive_chip.svg",
                50 => "fifty_chip.svg",
                100 => "onehundred_chip.svg",
                150 => "onefifty_chip.svg",
                _ => "default.svg",
            };
        }

        public void ClearHand()
        {
            CurrentHand.Clear();
            Currenthandvalue = 0;
        }


        private int handValue()
        {
            var total = 0;
            var numAces = 0;
            foreach (var hand in CurrentHand)
            {
                if(hand.Value == Value.Ace)
                {
                    total += 11;
                    numAces++;
                }
                else if(hand.Value == Value.Jack || hand.Value == Value.Queen || hand.Value == Value.King)
                {
                    total += 10;
                }
                else
                {
                    total += (int)hand.Value;
                }
            }

            while (total > 21 && numAces > 0)
            {
                total -= 10;
                numAces--;
            }

            return total;
        }

        public Cards getRandomCard()
        {
            var random = new Random();
            var randomSuit = (Suit)random.Next(0, 4);
            var randomValue = (Value)random.Next(1, 14);
            return GetImage(new Cards { Suit = randomSuit, Value = randomValue });
        }

        public static Cards GetImage(Cards c)
        {
            var image = new StringBuilder();

            switch (c.Value)
            {
                case Value.Ace:
                    image.Append("ace");
                    break;
                case Value.King:
                    image.Append("king");
                    break;
                case Value.Queen:
                    image.Append("queen");
                    break;
                case Value.Jack:
                    image.Append("jack");
                    break;
                case Value.Ten:
                    image.Append("ten");
                    break;
                case Value.Nine:
                    image.Append("nine");
                    break;
                case Value.Eight:
                    image.Append("eight");
                    break;
                case Value.Seven:
                    image.Append("seven");
                    break;
                case Value.Six:
                    image.Append("six");
                    break;
                case Value.Five:
                    image.Append("five");
                    break;
                case Value.Four:
                    image.Append("four");
                    break;
                case Value.Three:
                    image.Append("three");
                    break;
                case Value.Two:
                    image.Append("two");
                    break;
            }

            image.Append("_");

            switch (c.Suit)
            {
                case Suit.Apple:
                    image.Append("apple");
                    break;
                case Suit.Orange:
                    image.Append("orange");
                    break;
                case Suit.Pear:
                    image.Append("pear");
                    break;
                case Suit.Banana:
                    image.Append("banana");
                    break;
            }
            image.Append(".svg");
            c.Image.Source = image.ToString();

            return c;
        }
    }
}
