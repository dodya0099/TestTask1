public class Money : Pickup
{
    protected override void GetBonus()
    {
        levelManager.GetMoney();
    }
}
