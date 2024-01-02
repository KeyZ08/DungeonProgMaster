using UnityEngine;

public sealed class Skeleton : Unit
{
    public int Health;

    public Skeleton(Vector2Int position, int health = 100) : base(position, Tangibility.Obstacle) 
    {
        Health = health;
    }

    public override Unit GetCopy()
    {
        return new Skeleton(position, Health);
    }
}
