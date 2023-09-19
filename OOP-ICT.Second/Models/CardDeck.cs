namespace OOP_ICT.Models;

public class CardDeck
{
    private List<Card> _cards = new List<Card>();

    public CardDeck()
    {
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                Card newCard = new Card(rank: rank, suit: suit);
                _cards.Add(newCard);
            }
        }
    }

    public List<Card> GetDeck()
    {
        return _cards;
    }
}