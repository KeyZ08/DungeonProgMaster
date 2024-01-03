public sealed class SkeletonController : UnitController, IAttackable
{
    public void OnAttack(ContactDirection contact, GameContoller controller)
    {
        if (contact == ContactDirection.Side && unit is Skeleton skeleton)
        {
            skeleton.Health -= 50;
            if(skeleton.Health <= 0)
                Destroy(gameObject);
        }
    }
}