using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Orbyss.Components.JsonForms.Utils
{
    public static class DefaultJsonConverter
    {
        public static TValue Deserialize<TValue>(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());            
            return JsonConvert.DeserializeObject<TValue>(json, settings)!;
        }
    }
}
