using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace DPM.Domain
{
    public class Level
    {
        public readonly Map Map;
        public readonly Character Character;
        public readonly List<Unit> Units;

        [JsonConstructor]
        public Level([JsonProperty("Map")] Map map, [JsonProperty("Character")] Character character, [JsonProperty("Units")] List<Unit> units)
        {
            Map = map;
            Character = character;
            Units = units;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("Level:\n{");
            result.AppendLine(Map.ToString());
            result.AppendLine(Character.ToString());
            result.AppendLine("Units: {");
            for (int i = 0; i < Units.Count; i++)
                result.Append($"\t{Units[i]}\n");
            result.AppendLine("}");
            result.AppendLine("}");
            return result.ToString();
        }
    }
}
