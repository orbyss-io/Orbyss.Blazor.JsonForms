using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Orbyss.Blazor.JsonForms.Utils
{
    public static class ObjectJsonConverter
    {
        public static string Serialize(object @object)
        {
            var result = TryGetJToken(@object);
            return $"{result}";
        }

        public static JToken? ToJToken(this object @object)
        {
            return TryGetJToken(@object);
        }

        private static JToken? TryGetJToken(object? @object)
        {
            if (@object == null)
            {
                return null;
            }

            if (@object is JObject jObject)
            {
                return jObject;
            }

            if (@object is JsonObject jsonObject)
            {
                return JToken.Parse(jsonObject.ToJsonString() ?? "null");
            }

            if (@object is JsonDocument { RootElement: var rootElement })
            {
                return JToken.Parse(rootElement.GetRawText() ?? "null");
            }

            if (@object is JsonElement jsonElement)
            {
                return JToken.Parse(jsonElement.GetRawText() ?? "null");
            }

            if (@object is not JsonObject && @object is not JObject && @object is not JsonDocument && @object is not JsonElement)
            {
                throw new InvalidOperationException("Options is neither JObject nor JsonObject, nor JsonDocument, nor JsonElement");
            }

            return false;
        }
    }
}