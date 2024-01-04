public sealed class CoinController : UnitController<Coin>, IOnCome
{
    public void OnCome(ContactDirection contact, GameContoller controller)
    {
        if (contact == ContactDirection.Directly)
        {
            controller.coins += 1;
            Destroy(gameObject);
        }
    }
}