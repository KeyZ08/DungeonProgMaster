using Newtonsoft.Json;
using System.Text;
using UnityEngine;

public sealed class Chest : Unit
{
    public int Cost;

    public override string UnitId => "Chest";

    [JsonConstructor]
    public Chest([JsonProperty("Position")] Vector2Int position, [JsonProperty("Cost")] int cost = 10) : base(position, Tangibility.Obstacle)
    {
        Cost = cost;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append("Chest: { \t\t");
        result.Append($"Position: {Position}, ");
        result.Append($"Cost: {Cost}");
        result.Append(" }");
        return result.ToString();
    }
}
