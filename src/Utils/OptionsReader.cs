using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Orbyss.Blazor.JsonForms.Utils
{
    public static class OptionsReader
    {
        public static bool HasOption(object? options, string key) => TryGetOption(options, key, out _);

        public static JToken GetOption(object? options, string key) => TryGetOption(options, key, out var result)
        ? result!
            : throw new ArgumentException($"Options do not contain key '{key}'");

        private static bool TryGetOption(object? options, string key, out JToken? value)
        {
            value = null;

            if (options is null)
            {
                return false;
            }

            if (options is JObject jObject && jObject.ContainsKey(key))
            {
                value = jObject[key];
                return true;
            }

            if (options is JsonObject jsonObject && jsonObject.ContainsKey(key))
            {
                var @object = JToken.Parse($"{jsonObject.ToJsonString()}") as JObject;
                value = @object![key];
                return true;
            }

            if (options is JsonDocument jsonDocument)
            {
                var @object = JToken.Parse($"{jsonDocument.RootElement.GetRawText()}") as JObject;
                if (@object?.ContainsKey(key) == true)
                {
                    value = @object[key];
                    return true;
                }
            }

            if (options is JsonElement jsonElement)
            {
                var @object = JToken.Parse($"{jsonElement.GetRawText()}") as JObject;
                if (@object?.ContainsKey(key) == true)
                {
                    value = @object[key];
                    return true;
                }
            }

            if (options is not JsonObject
                && options is not JObject
                && options is not JsonDocument
                && options is not JsonElement)
            {
                throw new InvalidOperationException($"Options is neither JObject nor JsonObject, nor JsonDocument, nor JsonElement");
            }

            return false;
        }
    }
}