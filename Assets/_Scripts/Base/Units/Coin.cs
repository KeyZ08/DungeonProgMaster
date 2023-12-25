using UnityEngine;

public sealed class Coin : Unit
{
    public Coin(Vector2Int position) : base(position, Tangibility.None) { }

    public override Unit GetCopy()
    {
        return new Coin(position);
    }

    public override void OnAttack(ContactDirection contact, GameContoller controller)
    {
    }

    public override void OnCome(ContactDirection contact, GameContoller controller)
    {
        if (contact == ContactDirection.Directly)
        {
            controller.coins += 1;
        }
    }

    public override void OnTake(ContactDirection contact, GameContoller controller)
    {
    }
}