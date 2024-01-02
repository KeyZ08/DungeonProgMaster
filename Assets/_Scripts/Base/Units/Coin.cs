using UnityEngine;

public sealed class Coin : Unit
{
    public int Cost;

    public Coin(Vector2Int position, int cost = 1) : base(position, Tangibility.None) 
    { 
        Cost = cost;
    }

    public override Unit GetCopy()
    {
        return new Coin(position, Cost);
    }
}