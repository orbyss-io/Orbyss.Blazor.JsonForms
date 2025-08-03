using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System.Text.Json.Serialization;

using SystemTextJsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Orbyss.Components.JsonForms.Context.Translations
{
    public sealed record TranslationErrorSection(
        [
            property:
            JsonProperty(PropertyName = "custom", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("custom"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Custom,

       [
            property:
            JsonProperty(PropertyName = "const", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("const"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Const,

        [
            property:
            JsonProperty(PropertyName = "required", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("required"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Required,

       [
            property:
            JsonProperty(PropertyName = "minimum", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("minimum"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Minimum,

        [
            property:
            JsonProperty(PropertyName = "maximum", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("maximum"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Maximum,

        [
            property:
            JsonProperty(PropertyName = "minimumLength", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("minimumLength"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? MinimumLength,

        [
            property:
            JsonProperty(PropertyName = "maximumLength", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("maximumLength"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? MaximumLength,

        [
            property:
            JsonProperty(PropertyName = "minimmumItems", NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("minimmumItems"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? MinimumItems,

        [
            property:
            JsonProperty(PropertyName = "maximumItems",NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("maximumItems"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? MaximumItems,

        [
            property:
            JsonProperty(PropertyName = "contains",NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("contains"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Contains,

        [
            property:
            JsonProperty(PropertyName = "pattern",NullValueHandling = NullValueHandling.Ignore),
            JsonPropertyName("pattern"),
            SystemTextJsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)
        ]
        string? Pattern)
    {
        public string GetValue(ErrorType type)
        {
            return type switch
            {
                ErrorType.Minimum => GetValueOrDefault(Minimum, DefaultJsonFormValidationMessages.Minimum),
                ErrorType.Maximum => GetValueOrDefault(Maximum, DefaultJsonFormValidationMessages.Maximum),
                ErrorType.MinimumLength => GetValueOrDefault(MinimumLength, DefaultJsonFormValidationMessages.MinLength),
                ErrorType.MaximumLength => GetValueOrDefault(MaximumLength, DefaultJsonFormValidationMessages.MaxLength),
                ErrorType.MinimumItems => GetValueOrDefault(MinimumItems, DefaultJsonFormValidationMessages.MinItems),
                ErrorType.MaximumItems => GetValueOrDefault(MaximumItems, DefaultJsonFormValidationMessages.MaxItems),
                ErrorType.Contains => GetValueOrDefault(Contains, DefaultJsonFormValidationMessages.Contains),
                ErrorType.Required => GetValueOrDefault(Required, DefaultJsonFormValidationMessages.Required),
                ErrorType.Pattern => GetValueOrDefault(Pattern, DefaultJsonFormValidationMessages.Pattern),
                ErrorType.Const => GetValueOrDefault(Const, DefaultJsonFormValidationMessages.Const),

                _ => GetValueOrDefault(Custom, DefaultJsonFormValidationMessages.Default)
            };
        }

        string GetValueOrDefault(string? value, string defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (!string.IsNullOrWhiteSpace(Custom))
            {
                return Custom;
            }

            return defaultValue;
        }

        public static TranslationErrorSection DefaultSection()
        {
            return new (
               null, null, null, null, null, null, null, null, null, null, null
           );
        }
    }
}
