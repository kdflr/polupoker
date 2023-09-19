namespace OOP_ICT.Models;

public class Bank
{
    public void DeductMoney(BJPlayer player, int amount)
    {
        player.balance -= amount;
    }

    public void AddMoney(BJPlayer player, int amount)
    {
        player.balance += amount;
    }
    
    public void CheckBalance(BJPlayer player)
    {
        while (player.betSize > player.balance)
        {
            Console.WriteLine("Insufficient funds. Place a smaller bet");
            player.Bet(Convert.ToInt32(Console.ReadLine()));
        }
    }
}