using UnityEngine;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.App
{
    public sealed class CoinController : UnitController<Coin>, IOnCome
    {
        [SerializeField] private AudioClip coinSound;

        public void OnCome(ContactDirection contact, GameController controller)
        {
            if (contact == ContactDirection.Directly)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
                controller.collectedCoins += unit.Cost;
                Destroy(gameObject);
            }
        }
    }
}