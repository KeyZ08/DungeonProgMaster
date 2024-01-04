using UnityEngine;

public abstract class UnitController<TUnit> : BaseUnitController where TUnit : Unit
{
    protected TUnit unit;
    public override Tangibility Type => unit.Type;
    public override Vector2Int Position => unit.Position;

    public virtual void Construct(TUnit unit)
    {
        this.unit = unit;
    }
}
