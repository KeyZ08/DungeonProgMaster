using Newtonsoft.Json;
using System.Text;
using UnityEngine;

public class Character
{
    public readonly Vector2Int StartPosition;
    public Vector2Int CurrentPosition;

    public readonly Direction StartDirection;
    public Direction CurrentDirection;

    public Vector2Int Forward => CurrentDirection.Vector();

    [JsonConstructor]
    public Character([JsonProperty("Position")] Vector2Int startPosition, [JsonProperty("Direction")] Direction startDirection)
    {
        StartPosition = startPosition;
        CurrentPosition = startPosition;

        StartDirection = startDirection;
        CurrentDirection = startDirection;
    }

    public override string ToString()
    {
        var result = new StringBuilder("Character: { ");
        result.Append($"Position: {CurrentPosition}, ");
        result.Append($"Direction: {CurrentDirection}");
        result.Append(" }");
        return result.ToString();
    }
}
