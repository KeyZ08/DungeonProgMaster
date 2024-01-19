using UnityEngine;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.App
{
    public class ChestController : UnitController<Chest>, ITakeable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip openSound;
        private bool _beUsed = false;

        public bool beUsed => _beUsed;

        public void OnTake(ContactDirection contact, GameController controller)
        {
            if (contact == ContactDirection.Side)
            {
                _beUsed = true;
                AudioSource.PlayClipAtPoint(openSound, transform.position);
                Open();
                controller.collectedCoins += unit.Cost;
            }
        }

        private void Open()
        {
            animator.SetTrigger("Open");
        }
    }
}
