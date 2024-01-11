public sealed class SkeletonController : UnitController<Skeleton>, IAttackable
{
    public void OnAttack(ContactDirection contact, GameController controller, int damage)
    {
        if (contact == ContactDirection.Side)
        {
            unit.Health -= damage;
            if(unit.Health <= 0)
                Destroy(gameObject);
        }
    }
}