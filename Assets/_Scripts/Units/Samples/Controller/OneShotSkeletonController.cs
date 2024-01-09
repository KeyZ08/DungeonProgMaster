public sealed class OneShotSkeletonController : UnitController<Skeleton>, IAttackable
{
    public void OnAttack(ContactDirection contact, GameController controller)
    {
        if (contact == ContactDirection.Side)
        {
            Destroy(gameObject);
        }
    }
}