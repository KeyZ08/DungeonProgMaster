public sealed class SkeletonController : UnitController
{
    public override void OnAttack(ContactDirection contact, GameContoller controller)
    {
        if (contact == ContactDirection.Side)
        {
            unit.OnAttack(contact, controller);
            if ((unit as Skeleton).Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void OnCome(ContactDirection contact, GameContoller controller)
    {
        unit.OnCome(contact, controller);
    }

    public override void OnTake(ContactDirection contact, GameContoller controller)
    {
        unit.OnTake(contact, controller);
    }
}