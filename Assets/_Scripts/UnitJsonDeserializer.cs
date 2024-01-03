using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

public class BaseUnitConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(Unit).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null;
        return base.ResolveContractConverter(objectType);
    }
}

public class UnitJsonDeserializer : JsonConverter
{
    private Dictionary<string, Type> idToType;

    public UnitJsonDeserializer()
    {
        idToType = new Dictionary<string, Type>();

        var types = GetAllSubclassesOf(typeof(Unit)).ToArray();
        foreach (var type in types)
        {
            var property = type.GetProperty("UnitId");
            var id = property.GetValue(null).ToString();
            idToType.Add(id, type);
        }
    }

    private IEnumerable<Type> GetAllSubclassesOf(Type baseType)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
            foreach (var type in types)
            {
                yield return type;
            }
        }
    }

    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseUnitConverter() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Unit));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var id = jo["UnitId"].Value<string>();
        jo.Remove("UnitId");
        var type = idToType[id];
        return JsonConvert.DeserializeObject(jo.ToString(), type, SpecifiedSubclassConversion);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}