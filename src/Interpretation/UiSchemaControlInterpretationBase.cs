using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.UiSchema;

namespace Orbyss.Blazor.JsonForms.Interpretation
{
    public abstract class UiSchemaControlInterpretationBase(
        UiSchemaLabelInterpretation? labelInterpretation,
        bool readOnly,
        bool disabled,
        bool hidden,
        string relativeSchemaJsonPath,
        string absoluteSchemaJsonPath,
        string jsonPropertyName,
        string? absoluteParentSchemaJsonPath,
        FormUiSchemaElement element,
        UiSchemaRuleInterpretation? rule)

        : UiSchemaElementInterpretationBase(labelInterpretation)
    {
        public string AbsoluteSchemaJsonPath { get; } = absoluteSchemaJsonPath;

        public string JsonPropertyName { get; } = jsonPropertyName;

        public string? AbsoluteParentSchemaJsonPath { get; } = absoluteParentSchemaJsonPath;

        public bool ReadOnly { get; } = readOnly;

        public bool Disabled { get; } = disabled;

        public bool Hidden { get; } = hidden;

        public string RelativeSchemaJsonPath { get; } = relativeSchemaJsonPath;

        public JToken? GetOption(string key)
        {
            return element.HasOption(key)
                ? element.GetOption(key)
                : null;
        }

        public UiSchemaRuleInterpretation? Rule { get; } = rule;
    }
}