using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Chest : Unit
    {
        [JsonProperty("Cost", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(10)]
        public int Cost;

        public override string UnitId => "Chest";

        [JsonConstructor]
        public Chest([JsonProperty("Position")] Vector2Int position, int cost = 10) 
            : base(position, Tangibility.Obstacle)
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
}
