using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.Domain
{
    public sealed class Finish : Unit
    {
        [JsonConstructor]
        public Finish([JsonProperty("Position")] Vector2Int position) 
            : base(position, Tangibility.None){ }

        public override string UnitId => "Finish";

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Finish: { \t");
            result.Append($"Position: {Position}, ");
            result.Append(" }");
            return result.ToString();
        }
    }
}
