using UnityEngine;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.App
{
    public sealed class BoxController : UnitController<Box>, IAttackable
    {
        [SerializeField] private AudioClip damageSound;

        public void OnAttack(ContactDirection contact, GameController controller, int damage)
        {
            if (contact == ContactDirection.Side)
            {
                AudioSource.PlayClipAtPoint(damageSound, transform.position, 0.5f);
                unit.Health -= damage;
                if (unit.Health <= 0)
                    Destroy(gameObject);
            }
        }
    }
}