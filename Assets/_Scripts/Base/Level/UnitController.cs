using UnityEngine;

public abstract class UnitController : MonoBehaviour, IUnit
{
    protected Unit unit;
    protected GameContoller contoller;
    public Tangibility Type => unit.Type;
    public Vector2Int Position => unit.Position;

    public virtual void Construct(Unit unit, GameContoller contoller)
    {
        this.unit = unit;
        this.contoller = contoller;
    }

    public abstract void OnAttack(ContactDirection contact, GameContoller controller);

    public abstract void OnCome(ContactDirection contact, GameContoller controller);

    public abstract void OnTake(ContactDirection contact, GameContoller controller);

    private void OnDestroy()
    {
        contoller.OnUnitDestroy(this);
    }
}
