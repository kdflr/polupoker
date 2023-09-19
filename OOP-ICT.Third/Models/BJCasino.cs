namespace OOP_ICT.Models;

public class BJCasino
{
    public Bank activeBank = new Bank();
    
    public void WinDoubleBet(BJPlayer player)
    {
        activeBank.AddMoney(player, 2 * player.betSize);
    }

    public void WinBet(BJPlayer player)
    {
        activeBank.AddMoney(player, player.betSize);
    }

    public void LoseBet(BJPlayer player)
    {
        activeBank.DeductMoney(player, player.betSize);
    }
}