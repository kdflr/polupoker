namespace OOP_ICT.Models;

public class PkPlayer
{
    public int pid;
    public int balance;
    public int betSize;
    public bool active = true;
    public string endgameStatus;
    public List<Card> _hand = new List<Card>();

    public PkPlayer(int playerId)
    {
        pid = playerId;
    }
    
    void JoinGame(PkPlayer player, PkGame game)
    {
        game._players.Add(player);
    }

    public void Bet(int size)
    {
        betSize = size;
    }

    public void Raise(int size)
    {
        betSize += size;
    }
}