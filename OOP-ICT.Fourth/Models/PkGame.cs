using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Security.Authentication.ExtendedProtection;

namespace OOP_ICT.Models;

public class PkGame
{
    private int gid;
    public List<PkPlayer> _players = new List<PkPlayer>();
    private bool betting = true;
    private PkDealerAsPlayer activeDealer = new PkDealerAsPlayer();
    private PkCasino casino = new PkCasino();
    private List<Card> _table = new List<Card>();

    public PkGame(int gameId)
    {
        gid = gameId;
    }

    void PlayGame()
    {
        foreach (PkPlayer player in _players)
        {
            player._hand.Add(activeDealer.GiveCard());
            player._hand.Add(activeDealer.GiveCard());
        }
        
        _players[0].Bet(Convert.ToInt32(Console.ReadLine()));
        
        BettingRound(); // preflop
        
        for (int i = 0; i < 3; i++)
        {
            _table.Add(activeDealer.GiveCard()); // flop
        }

        for (int i = 0; i < 2; i++)
        {
            BettingRound();
            _table.Add(activeDealer.GiveCard()); // turn and river
        }

        int bestHand = 0;
        foreach (PkPlayer player in _players) // showdown
        {
            if (player.active)
            {
                player._hand.AddRange(_table);
                int handValue = (int)CheckHand(player._hand);
                if (handValue >= bestHand)
                {
                    bestHand = handValue;
                }
            }
        }

        int nWinners = 0;
        foreach (PkPlayer player in _players)
        {
            if ((int)CheckHand(player._hand) == bestHand)
            {
                player.endgameStatus = "win";
                nWinners += 1;
            }
            else
            {
                player.endgameStatus = "lose";
            }
        }

        foreach (PkPlayer player in _players)
        {
            casino.WinMoney(player, nWinners);
        }
    }
    
    void BettingRound()
    {
        int currMaxBet = _players[0].betSize;
        betting = true;
        while (betting)
        {
            foreach (PkPlayer player in _players)
            {
                string request = Console.ReadLine();

                if (request == "raise")
                {
                    int raiseAmt = Convert.ToInt32(Console.ReadLine());
                    if (player.betSize + raiseAmt <= player.balance)
                    {
                        player.Raise(raiseAmt);
                        currMaxBet = player.betSize;
                        casino.activeBank.DeductMoney(player, raiseAmt);
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds. Raise a smaller amount or fold.");
                    }
                }
                else if (request == "call")
                {
                    if (currMaxBet <= player.balance)
                    {
                        casino.activeBank.DeductMoney(player, currMaxBet - player.betSize);
                        player.betSize = currMaxBet;
                    }
                }
                else if (request == "fold")
                {
                    player.active = false;
                }
            }

            int checkd = _players.Count; // checking if all the players have the same bet placed
            int zeroBalance = _players.Count; // checking if all the players have drained their funds
            foreach (PkPlayer player in _players)
            {
                if (player.betSize == currMaxBet)
                {
                    checkd -= 1;
                }

                if (player.balance - player.betSize == 0)
                {
                    zeroBalance -= 1;
                }
            }

            if (checkd == 0 || zeroBalance == 0)
            {
                betting = false;
            }
            
            CheckActivePlayers();
            
        }
    }
    

    void CheckActivePlayers()
    {
        int activePlayers = _players.Count();
        foreach (PkPlayer player in _players)
        {
            if (player.active == false)
            {
                activePlayers -= 1;
            } 
        }

        if (activePlayers == 1)
        {
            betting = false;
            foreach (PkPlayer player in _players)
            {
                if (player.active)
                {
                    player.balance += casino.activeBank.pot;
                }
            }
        }
    }

    Hand CheckHand(List<Card> finalHand)
    {
        bool suitMatching = CheckForSuit(finalHand);
        bool sequencesFound = CheckForSequences(finalHand);
        (int, int) reps = CheckForRepeats(finalHand);
        Hand strongestHand = new Hand();
        finalHand.Sort();
        if (sequencesFound)
        {
            if (suitMatching)
            {
                if (finalHand[0].rank == Rank.Ten)
                {
                    strongestHand = Hand.RoyalFlush;
                }
                else
                {
                    strongestHand = Hand.StraightFlush;
                }
            }
            else
            {
                strongestHand = Hand.Straight;
            }
        }
        else
        {
            if (reps.Item1 == 4)
            {
                strongestHand = Hand.FoaK;
            }
            else if (reps.Item1 == 3)
            {
                if (reps.Item2 == 2)
                {
                    strongestHand = Hand.FullHouse;
                }
                else
                {
                    strongestHand = Hand.ToaK;
                }
            }
            else if (reps.Item1 == 2)
            {
                if (reps.Item2 == 2)
                {
                    strongestHand = Hand.TwoPair;
                }
                else
                {
                    strongestHand = Hand.Pair;
                }
            }
            else
            {
                strongestHand = Hand.HighCard;
            }
        }

        return strongestHand;
    }

    bool CheckForSuit(List<Card> finalHand)
    {
        List<int> suits = new List<int>();
        foreach (Card card in finalHand)
        {
            suits.Add((int)card.suit);
        }
        
        suits.Sort();

        for (int i = 0; i < 3; i++)
        {
            int matches = 0;
            for (int j = 1; j < 5; j++ )
            {
                if (suits[i] == suits[i + j])
                {
                    matches += 1;
                }
            }

            if (matches == 4)
            {
                return true;
            }
        }
        return false;
    }
    
    (int, int) CheckForRepeats(List<Card> finalHand)
    {
        List<int> ranks = new List<int>();
        List<int> repeats = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        foreach (Card card in finalHand)
        {
            ranks.Add((int)card.rank);
        }
        ranks.Sort();

        for (int i = 0; i < ranks.Count; i++)
        {
            for (int j = 0; j < ranks.Count; j++)
            {
                if (ranks[i] == ranks[j])
                {
                    repeats[i] += 1;
                }
            }
        }

        int max = repeats.Max();
        int secondMax = repeats.Where(x => x < max).Max();
        (int, int) maxes = (max, secondMax);
        return maxes;
    }

    bool CheckForSequences(List<Card> finalHand)
    {
        List<int> ranks = new List<int>();
        foreach (Card card in finalHand)
        {
            ranks.Add((int)card.rank);
        }
        ranks.Sort();

        for (int i = 0; i < 3; i++)
        {
            int lenSeq = 0;
            for (int j = 1; j < 5; j++)
            {
                if (ranks[i + j] == ranks[i] + j)
                {
                    lenSeq += 1;
                }
            }

            if (lenSeq == 5)
            {
                return true;
            }
        }

        return false;
    }
}