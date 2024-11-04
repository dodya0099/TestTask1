public class Bottle : Pickup
{
    protected override void GetBonus()
    {
        levelManager.GetDrink();
    }
}
