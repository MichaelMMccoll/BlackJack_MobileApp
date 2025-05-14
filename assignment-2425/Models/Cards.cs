namespace assignment_2425.Models
{
    public class Cards
    {
        public Suit Suit { get; set; }
        public Value Value { get; set; }
        public Image Image { get; set; } =new Image();
    }

    public enum Suit
    {
        Banana,
        Apple,
        Orange,
        Pear
    };

    public enum Value
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    };
}
