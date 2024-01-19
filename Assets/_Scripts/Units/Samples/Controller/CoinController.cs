using UnityEngine;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.App
{
    public sealed class CoinController : UnitController<Coin>, IOnCome, IShouldBeUsed
    {
        [SerializeField] private AudioClip coinSound;
        private bool _beUsed = false;

        public bool wasUsed => _beUsed;

        public void OnCome(ContactDirection contact, GameController controller)
        {
            if (contact == ContactDirection.Directly)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
                controller.collectedCoins += unit.Cost;
                _beUsed = true;
                Destroy(gameObject);
            }
        }
    }
}