using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Skeleton : Unit
    {
        [JsonProperty("Health", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(100)]
        public int Health;

        public override string UnitId => "Skeleton";

        [JsonConstructor]
        public Skeleton([JsonProperty("Position")] Vector2Int position, int health = 100) 
            : base(position, Tangibility.Obstacle)
        {
            Health = health;
        }

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
