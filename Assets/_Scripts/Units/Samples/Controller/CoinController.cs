public sealed class CoinController : UnitController<Coin>, IOnCome
{
    public void OnCome(ContactDirection contact, GameController controller)
    {
        if (contact == ContactDirection.Directly)
        {
            controller.coins += 1;
            Destroy(gameObject);
        }
    }
}