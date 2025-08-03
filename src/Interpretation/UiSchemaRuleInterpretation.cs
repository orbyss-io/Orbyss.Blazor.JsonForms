using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class UiSchemaRuleInterpretation(string absoluteJsonSchemaPath, JSchema schema, UiSchemaElementRuleEffect effect)       
    {
        public string AbsoluteJsonSchemaPath { get; } = absoluteJsonSchemaPath;

        public JSchema Schema { get; } = schema;

        public UiSchemaElementRuleEffect Effect { get; } = effect;
    }
}
