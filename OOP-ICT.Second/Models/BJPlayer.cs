namespace OOP_ICT.Models;

public class BJPlayer
{
    public int pid;
    public int balance;
    public int betSize;
    public int score = 0;
    public bool active = true;
    public string endgameStatus;
    public List<Card> _hand = new List<Card>();

    public BJPlayer(int playerId)
    {
        pid = playerId;
    }
    
    void JoinGame(BJPlayer player, BJGame game)
    {
        game._players.Add(player);
    }

    public void Bet(int size)
    {
        betSize = size;
        balance -= betSize;
    }
}