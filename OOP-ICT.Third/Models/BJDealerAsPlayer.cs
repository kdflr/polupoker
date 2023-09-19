namespace OOP_ICT.Models;

public class BJDealerAsPlayer : Dealer
{
    public List<Card> _hand = new List<Card>();
    public int balance;
    public int score = 0;
}