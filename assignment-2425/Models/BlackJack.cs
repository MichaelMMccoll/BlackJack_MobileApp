using assignment_2425.Helpers;
using assignment_2425.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace assignment_2425.ViewModels
{
    public partial class BlackJack : ObservableObject
    {
        [ObservableProperty]
        public Player dealer = new Player();
        [ObservableProperty]
        public Player player = new Player();
        public Deck Deck { get; set; } = new Deck();
        public SmartPlayer SmartPlayer { get; set; } = new SmartPlayer();

        public ICommand DrawTillSevenTeen { get; }

        public BlackJack()
        {
            DrawTillSevenTeen = new Command(async () => await DealerDraw());
            Initialhand();
        }


        //Gives a card either to dealer or player
        public void GetNewCard(string user)
        {
            if (!SmartPlayer.IsSmart)
            {
                if (user == "player")
                {
                    Player.CurrentHand.Add(getRandomCard());

                    Player.Currenthandvalue = playerHandValue();
                }
                else
                {
                    Dealer.CurrentHand.Add(getRandomCard());

                    Dealer.Currenthandvalue = dealerHandValue();
                }
            }
            else
            {
                if (user == "player")
                {
                    Player.CurrentHand.Add(Deck.Cards.First());
                    Player.Currenthandvalue = playerHandValue();
                    Deck.Cards.RemoveAt(0);
                }
                else
                {
                    Dealer.CurrentHand.Add(Deck.Cards.First());
                    Dealer.Currenthandvalue = dealerHandValue();
                    Deck.Cards.RemoveAt(0);
                }
            }
        }

        //Generates a inital hand for player and dealer
        public void Initialhand()
        {
            try
            {
                //Game logic if the user is playing on normal mode
                if (!SmartPlayer.IsSmart)
                {
                    //player
                    Player.CurrentHand.Clear();
                    Player.Currenthandvalue = 0;
                    Player.CurrentHand.Add(getRandomCard());
                    Player.CurrentHand.Add(getRandomCard());

                    Player.Currenthandvalue = playerHandValue();

                    //dealer
                    Dealer.CurrentHand.Clear();
                    Dealer.Currenthandvalue = 0;
                    Dealer.CurrentHand.Add(getRandomCard());
                    Dealer.CurrentHand.Add(getRandomCard());

                    Dealer.Currenthandvalue = dealerHandValue();
                }
                else
                {
                    //Game logic if on hardMode (IsSmart), this will play with a real deck of cards
                    if (Deck.Cards.Count <= 4)
                    {
                        Deck = new Deck();
                    }
                    //player
                    Player.CurrentHand.Clear();
                    Player.Currenthandvalue = 0;
                    Player.CurrentHand.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                    Player.CurrentHand.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);

                    Player.Currenthandvalue = playerHandValue();

                    //dealer
                    Dealer.CurrentHand.Clear();
                    Dealer.Currenthandvalue = 0;
                    Dealer.CurrentHand.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                    Dealer.CurrentHand.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);

                    Dealer.Currenthandvalue = dealerHandValue();
                }

                Player.CurrentHand.First().Image.SetValue(SemanticProperties.DescriptionProperty, Player.Currenthandvalue);
            }
            catch (Exception)
            {
                Deck = new Deck();
                Initialhand();
            }
        }

        //returns a random card void of deck
        public Cards getRandomCard()
        {
            var random = new Random();
            var randomSuit = (Suit)random.Next(0, 4);
            var randomValue = (Value)random.Next(1, 14);
            return Player.GetImage(new Cards { Suit = randomSuit, Value = randomValue });
        }

        //calculates player current hand value
        private int playerHandValue()
        {
            var total = 0;
            var numAces = 0;
            foreach (var hand in Player.CurrentHand)
            {
                if (hand.Value == Value.Ace)
                {
                    total += 11;
                    numAces++;
                }
                else if (hand.Value == Value.Jack || hand.Value == Value.Queen || hand.Value == Value.King)
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

        //calculates dealers hand value
        private int dealerHandValue()
        {
            var total = 0;
            var numAces = 0;
            foreach (var hand in Dealer.CurrentHand)
            {
                if (hand.Value == Value.Ace)
                {
                    total += 11;
                    numAces++;
                }
                else if (hand.Value == Value.Jack || hand.Value == Value.Queen || hand.Value == Value.King)
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

        //dealer gets a new card taking into acount bool
        public async Task DealerDraw()
        {
            //If on normal mode dealer takes cards based on normal rules
            if (!SmartPlayer.IsSmart)
            {
                while (Dealer.Currenthandvalue <= 17)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        GetNewCard("dealer");
                    });

                    await Task.Delay(1000);
                }
            }
            else
            {
                //Deler uses a RiskVariable to take card or not this requires IsSmart to be turned on
                while (true && Dealer.Currenthandvalue <= 21)
                {
                    var maxNum = 21-Dealer.Currenthandvalue;
                    if (Dealer.Currenthandvalue >= Player.Currenthandvalue || Dealer.Currenthandvalue>=21)
                    {
                        break;
                    }
                    //If there are any cards of valid value ie what dealer needs before going bust
                    if (Deck.Cards.Any(x => (int) x.Value >= maxNum))
                    {
                        var count = 0;
                        if(maxNum == 10)
                        {
                            count = Deck.Cards.Count;
                        }
                        else
                        {
                            count = Deck.Cards.Count(x => (int)x.Value <= maxNum);
                        }

                        if (count/Deck.Cards.Count > SmartPlayer.RiskVariable)
                        {
                            break;
                        }
                    }
                    GetNewCard("dealer");
                }
            }
        }
    }
}
