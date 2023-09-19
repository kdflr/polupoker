namespace OOP_ICT.Models;

public class Card
{
    public Rank rank;
    public Suit suit;
    public Card(Rank rank, Suit suit)
    {
        this.rank = rank;
        this.suit = suit;
    }
}