using UnityEngine;

public sealed class CoinController : UnitController<Coin>, IOnCome
{
    [SerializeField] private AudioClip coinSound;

    public void OnCome(ContactDirection contact, GameController controller)
    {
        if (contact == ContactDirection.Directly)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            controller.coins += unit.Cost;
            Destroy(gameObject);
        }
    }
}