using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    protected Unit unit;
    public Tangibility Type => unit.Type;
    public Vector2Int Position => unit.Position;

    public virtual void Construct(Unit unit)
    {
        this.unit = unit;
    }

    private void OnDestroy()
    {
        FindAnyObjectByType<GameContoller>().OnUnitDestroy(this);
    }
}
