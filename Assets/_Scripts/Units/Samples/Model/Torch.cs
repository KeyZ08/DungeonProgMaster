using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Torch : Unit
    {
        [JsonConstructor]
        public Torch([JsonProperty("Position")] Vector2Int position) : base(position, Tangibility.None) { }

        public override string UnitId => "Torch";

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Torch: { \t");
            result.Append($"Position: {Position}, ");
            result.Append(" }");
            return result.ToString();
        }
    }
}
