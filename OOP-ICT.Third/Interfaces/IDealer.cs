using OOP_ICT.Models;

namespace OOP_ICT.Interfaces;

// Todo: возможна и другая сигнатура Dealer, я тут скорее просто показываю примерный план работы
public interface IDealer
{
    void InitializeCardDeck();
    UserDeck CreateShuffledUserDeck();
    Card GiveCard();
}