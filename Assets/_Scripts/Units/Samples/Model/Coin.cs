using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Coin : Unit
    {
        [JsonProperty("Cost", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(1)]
        public int Cost;

        public override string UnitId => "Coin";

        [JsonConstructor]
        public Coin([JsonProperty("Position")] Vector2Int position, int cost = 1) 
            : base(position, Tangibility.None)
        {
            Cost = cost;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Coin: { \t\t");
            result.Append($"Position: {Position}, ");
            result.Append($"Cost: {Cost}");
            result.Append(" }");
            return result.ToString();
        }
    }
}