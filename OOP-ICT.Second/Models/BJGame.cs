namespace OOP_ICT.Models;

public class BJGame
{
    private int gid;
    public List<BJPlayer> _players = new List<BJPlayer>();
    private bool isGoing = true;
    private BJDealerAsPlayer activeDealer = new BJDealerAsPlayer();

    public BJGame(int gameId)
    {
        gid = gameId;
    }

    void PlayGame()
    {
        BettingRound();
        ReceiveCards();
        while (isGoing)
        {
            GetCardRequests();
        }
        DealersTurn();
        Showdown();
    }

    void BettingRound()
    {
        foreach (BJPlayer player in _players)
        {
            player.Bet(Convert.ToInt32(Console.ReadLine()));
        }
    }

    void ReceiveCards()
    {
        activeDealer._hand.Add(activeDealer.GiveCard());

        foreach (BJPlayer player in _players)
        {
            player._hand.Add(activeDealer.GiveCard());
            player._hand.Add(activeDealer.GiveCard());
        }
    }

    void GetCardRequests()
    {
        CheckForBusts();
        foreach (BJPlayer player in _players)
        {
            if (player.active)
            {
                string request = Console.ReadLine();
                if (request == "hit")
                {
                    player._hand.Add(activeDealer.GiveCard());
                }

                if (request == "stand")
                {
                    player.active = false;
                }
            }
        }

        GetActivePlayers();
    }

    void CheckForBusts()
    {
        foreach (BJPlayer player in _players)
        {
            foreach (Card card in player._hand)
            {
                player.score += (int)card.rank;
            }

            if (player.score > 21)
            {
                player.active = false;
            }
        }
    }

    void GetActivePlayers()
    {
        int activePlayers = _players.Count;

        foreach (BJPlayer player in _players) //count active players
        {
            if (player.active == false)
            {
                activePlayers -= 1;
            }
        }

        if (activePlayers == 0)
        {
            isGoing = false;
        }
    }

    void DealersTurn()
    {
        while (activeDealer.score < 17)
        {
            activeDealer._hand.Add(activeDealer.GiveCard());
            foreach (Card card in activeDealer._hand)
            {
                activeDealer.score += (int)card.rank;
            }
        }
    }

    void Showdown()
    {
        if (activeDealer.score > 21)
        {
            foreach (BJPlayer player in _players)
            {
                if (player.score <= 21)
                {
                    player.endgameStatus = "win";
                }
                else
                {
                    player.endgameStatus = "lose";
                }
            }
        }
        else if (activeDealer.score == 21)
        {
            foreach (BJPlayer player in _players)
            {
                if (player.score == 21)
                {
                    player.endgameStatus = "push";
                }
                else
                {
                    player.endgameStatus = "lose";
                }
            }
        }
        else
        {
            foreach (BJPlayer player in _players)
            {
                if (player.score <= 21)
                {
                    if (player.score > activeDealer.score)
                    {
                        player.endgameStatus = "win";
                    }
                    else if (player.score == activeDealer.score)
                    {
                        player.endgameStatus = "push";
                    }
                    else
                    {
                        player.endgameStatus = "lose";
                    }
                }
                else
                {
                    player.endgameStatus = "lose";
                }
            }  
        }

        foreach (BJPlayer player in _players)
        {
            if (player.endgameStatus == "win")
            {
                player.balance += 2 * player.betSize;
            }
            else if (player.endgameStatus == "push")
            {
                player.balance += player.betSize;
            }
        }
    }
}
