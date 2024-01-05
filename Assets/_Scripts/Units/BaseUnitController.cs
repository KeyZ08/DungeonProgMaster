using UnityEngine;

public abstract class BaseUnitController : MonoBehaviour
{
    public abstract Tangibility Type { get; }
    public abstract Vector2Int Position { get; }

    private void OnDestroy()
    {
        FindAnyObjectByType<GameController>()?.OnUnitDestroy(this);
    }
}
