namespace OOP_ICT.Models;

public class PkCasino
{
    public Bank activeBank = new Bank();

    public void WinMoney(PkPlayer player, int nWinners)
    {
        player.balance += activeBank.pot / nWinners;
    }
}