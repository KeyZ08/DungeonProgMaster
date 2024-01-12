using UnityEngine;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.App
{
    public sealed class SkeletonController : UnitController<Skeleton>, IAttackable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip damageSound;

        public void OnAttack(ContactDirection contact, GameController controller, int damage)
        {
            if (contact == ContactDirection.Side)
            {
                animator.SetTrigger("Damage");
                AudioSource.PlayClipAtPoint(damageSound, transform.position);
                unit.Health -= damage;
                if (unit.Health <= 0)
                    Destroy(gameObject);
            }
        }
    }
}