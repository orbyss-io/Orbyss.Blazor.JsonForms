using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Utils;
using System.Text.Json.Serialization;

namespace Orbyss.Blazor.JsonForms.UiSchema
{
    public sealed record FormUiSchemaElement(
           [property: JsonProperty(PropertyName = "type"), JsonPropertyName("type")] UiSchemaElementType Type,
           [property: JsonProperty(PropertyName = "label"), JsonPropertyName("label")] string? Label,
           [property: JsonProperty(PropertyName = "i18n"), JsonPropertyName("i18n")] string? I18n,
           [property: JsonProperty(PropertyName = "elements"), JsonPropertyName("elements")] FormUiSchemaElement[] Elements,
           [property: JsonProperty(PropertyName = "scope"), JsonPropertyName("scope")] string? Scope,
           [property: JsonProperty(PropertyName = "rule"), JsonPropertyName("rule")] UiSchemaElementRule? Rule,
           [property: JsonProperty(PropertyName = "options"), JsonPropertyName("options")] object? Options)
    {
        public bool HasOption(string key) => OptionsReader.HasOption(Options, key);

        public JToken GetOption(string key) => OptionsReader.GetOption(Options, key);

        public bool HasChildElements => Elements?.Length > 0;
    }
}