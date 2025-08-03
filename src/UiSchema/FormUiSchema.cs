using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orbyss.Components.JsonForms.Helpers;
using System.Text.Json.Serialization;

namespace Orbyss.Components.JsonForms.UiSchema
{
    public sealed record FormUiSchema(
        [property: JsonProperty, JsonPropertyName("type")] UiSchemaElementType Type, 
        [property: JsonProperty, JsonPropertyName("scope")] string? Scope, 
        [property: JsonProperty, JsonPropertyName("label")] string? Label, 
        [property: JsonProperty, JsonPropertyName("elements")] FormUiSchemaElement[] Elements, 
        [property: JsonProperty, JsonPropertyName("options")] object? Options)
    {
        public bool HasOption(string key)
        {
            return OptionsReader.HasOption(Options, key);
        }

        public JToken GetOption(string key)
        {
            return OptionsReader.GetOption(Options, key);
        }
    }
}
