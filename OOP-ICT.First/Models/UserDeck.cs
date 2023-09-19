namespace OOP_ICT.Models;

public class UserDeck
{
    private Queue<Card> _cards;
    public UserDeck(Queue<Card> cards)
    {
        _cards = cards;
    }
    public Queue<Card> GetCards()
    {
        return _cards;
    }
}