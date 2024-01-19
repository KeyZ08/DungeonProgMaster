using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Box : Unit
    {
        [JsonProperty("Health", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(100)]
        public int Health;

        public override string UnitId => "Box";

        [JsonConstructor]
        public Box([JsonProperty("Position")] Vector2Int position, int health = 100) 
            : base(position, Tangibility.Obstacle)
        {
            Health = health;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Box: { \t");
            result.Append($"Position: {Position}, ");
            result.Append($"Health: {Health}");
            result.Append(" }");
            return result.ToString();
        }
    }
}
