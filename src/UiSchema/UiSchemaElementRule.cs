using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Orbyss.Blazor.JsonForms.UiSchema;

public record UiSchemaElementRule(
    [property: JsonProperty(PropertyName = "condition"), JsonPropertyName("condition")] UiSchemaElementRuleCondition Condition,
    [property: JsonProperty(PropertyName = "effect"), JsonPropertyName("effect")] UiSchemaElementRuleEffect Effect
);