using OOP_ICT.Interfaces;

namespace OOP_ICT.Models;

public class Dealer : IDealer
{
    private CardDeck _initDeck;
    private UserDeck _playingDeck;
    
    public void InitializeCardDeck()
    {
        _initDeck = new CardDeck();
    }
    
    public UserDeck CreateShuffledUserDeck()
    {
        List<Card> newDeck = _initDeck.GetDeck();
        List<Card> shuffledList = Shuffle(newDeck);
        Queue<Card> shuffledDeck = new Queue<Card>();
        foreach (Card card in newDeck)
        {
            shuffledDeck.Enqueue(card);
        }
        _playingDeck = new UserDeck(shuffledDeck);
        return _playingDeck;
    }
    
    public Card GiveCard()
    {
        Card topCard = _playingDeck.GetCards().Dequeue();
        return topCard;
    }
    
    private List<Card> Shuffle(List<Card> initDeck)
    {
        List<Card> shuffledDeck = new List<Card>();
        int halfSize = 26;
        for (int j = 0; j < halfSize; j++)
        {
            shuffledDeck.Add(initDeck[j]);
            shuffledDeck.Add(initDeck[j + halfSize]);
        }
        return shuffledDeck;
    }
}