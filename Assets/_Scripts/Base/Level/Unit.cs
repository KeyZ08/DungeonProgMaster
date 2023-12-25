using UnityEngine;

public abstract class Unit: IUnit
{
    protected Tangibility type;
    protected Vector2Int position;

    public Unit(Vector2Int position, Tangibility type = Tangibility.None)
    {
        this.position = position;
        this.type = type;
    }

    public abstract Unit GetCopy();

    public Tangibility Type => type;
    public Vector2Int Position => position;

    public abstract void OnAttack(ContactDirection contact, GameContoller controller);

    public abstract void OnCome(ContactDirection contact, GameContoller controller);

    public abstract void OnTake(ContactDirection contact, GameContoller controller);
}