using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;


namespace DPM.Domain
{
    [JsonConverter(typeof(UnitJsonDeserializer))]
    public abstract class Unit
    {
        protected Tangibility type;
        protected Vector2Int position;

        [JsonConstructor]
        public Unit([JsonProperty("Position")] Vector2Int position, [JsonProperty("Type")] Tangibility type = Tangibility.None)
        {
            this.position = position;
            this.type = type;
        }

        public Tangibility Type => type;

        public Vector2Int Position => position;

        public abstract string UnitId { get; }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("Unit: { \t\t");
            result.Append($"UnitId: {UnitId}, ");
            result.Append($"Position: {position}");
            result.Append($"Tangibility: {type}, ");
            result.Append(" }");
            return result.ToString();
        }
    }
}