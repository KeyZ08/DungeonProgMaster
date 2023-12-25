public sealed class CoinController : UnitController
{
    public override void OnAttack(ContactDirection contact, GameContoller controller)
    {
        unit.OnAttack(contact, controller);
    }

    public override void OnCome(ContactDirection contact, GameContoller controller)
    {
        unit.OnCome(contact, controller);
        Destroy(gameObject);
    }

    public override void OnTake(ContactDirection contact, GameContoller controller)
    {
        unit.OnTake(contact, controller);
    }
}