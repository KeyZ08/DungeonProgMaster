using UnityEngine;

public sealed class Skeleton : Unit
{
    private int health;
    public int Health => health;

    public Skeleton(Vector2Int position, int health = 100) : base(position, Tangibility.Obstacle) 
    {
        this.health = health;
    }

    public override void OnAttack(ContactDirection contact, GameContoller controller)
    {
        if (contact == ContactDirection.Side)
        {
            health -= 50;
        }
    }

    public override void OnCome(ContactDirection contact, GameContoller controller)
    {
    }

    public override void OnTake(ContactDirection contact, GameContoller controller)
    {
    }

    public override Unit GetCopy()
    {
        return new Skeleton(position, health);
    }
}
