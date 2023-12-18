using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Object", menuName = "Scriptable Object")]
public class ScriptableUnit : ScriptableObject
{
    public Kind Kind;
    public BaseUnit UnitPrefab;
}

public enum Kind
{
    Enemy,
    PickableObject
}
