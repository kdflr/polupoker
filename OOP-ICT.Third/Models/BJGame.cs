using System.Diagnostics.Tracing;

namespace OOP_ICT.Models;

public class BJGame
{
    private int gid;
    public List<BJPlayer> _players = new List<BJPlayer>();
    private bool isGoing = true;
    private BJDealerAsPlayer activeDealer = new BJDealerAsPlayer();
    private BJCasino casino = new BJCasino();

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
            casino.activeBank.CheckBalance(player); // collecting bets
        }
    }

    void ReceiveCards() // giving out a card to the dealer and 2 cards to each player
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
        foreach (BJPlayer player in _players) // getting stand or hit requests from each player
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
        foreach (BJPlayer player in _players) // check if any of the players has busted
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

        foreach (BJPlayer player in _players) //count players still not busted and not standing
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
        while (activeDealer.score < 17) // dealer opens up cards until he surpasses 17 points
        {
            activeDealer._hand.Add(activeDealer.GiveCard());
            foreach (Card card in activeDealer._hand)
            {
                activeDealer.score += (int)card.rank;
            }
        }
    }

    void Showdown() // counting the points, could be optimized
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

        foreach (BJPlayer player in _players) // adding or deducting funds
        {
            if (player.endgameStatus == "win")
            {
                casino.WinDoubleBet(player);
            }
            else if (player.endgameStatus == "push")
            {
                casino.WinBet(player);
            }
            else
            {
                casino.LoseBet(player);
            }
        }
    }
}
