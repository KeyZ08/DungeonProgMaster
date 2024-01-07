
using UnityEngine;

public class TransformParameters
{
    public Transform Parent;
    public Vector2 Position;
    public Quaternion Rotation;

    public TransformParameters(Transform parent, Vector2 position, Quaternion rotation = default)
    {
        Parent = parent;
        Position = position;
        Rotation = rotation;
    }
}