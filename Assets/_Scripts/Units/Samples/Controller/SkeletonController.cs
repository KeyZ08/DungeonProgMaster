public sealed class SkeletonController : UnitController<Skeleton>, IAttackable
{
    public void OnAttack(ContactDirection contact, GameController controller)
    {
        if (contact == ContactDirection.Side)
        {
            unit.Health -= 50;
            if(unit.Health <= 0)
                Destroy(gameObject);
        }
    }
}