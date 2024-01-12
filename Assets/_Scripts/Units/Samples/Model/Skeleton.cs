using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Skeleton : Unit
    {
        public int Health;

        [JsonConstructor]
        public Skeleton([JsonProperty("Position")] Vector2Int position, [JsonProperty("Health")] int health = 100) : base(position, Tangibility.Obstacle)
        {
            Health = health;
        }

        public override string UnitId => "Skeleton";

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Skeleton: { \t");
            result.Append($"Position: {Position}, ");
            result.Append($"Health: {Health}");
            result.Append(" }");
            return result.ToString();
        }
    }
}
