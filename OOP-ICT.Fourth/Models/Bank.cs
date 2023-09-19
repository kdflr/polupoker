namespace OOP_ICT.Models;

public class Bank
{
    public int pot;

    public void AddMoney(PkPlayer player, int amount)
    {
        player.balance += amount;
        pot -= amount;
    }

    public void DeductMoney(PkPlayer player, int amount)
    {
        player.balance -= amount;
        pot += amount;
    }
}