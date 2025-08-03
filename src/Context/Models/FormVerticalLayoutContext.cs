using Newtonsoft.Json.Linq;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Interpretation;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed class FormVerticalLayoutContext(UiSchemaVerticalLayoutInterpretation interpretation, IEnumerable<IFormElementContext> rows)
        : FormElementContextBase<UiSchemaVerticalLayoutInterpretation>(interpretation)
    {
        public IEnumerable<IFormElementContext> Rows { get; } = rows;

        public override bool ReadOnly => false;

        protected override bool DisabledCore => false;

        protected override bool HiddenCore => false;

        public override bool FindDataPathBySchemaPath(string schemaPath, out string dataPath)
        {
            dataPath = string.Empty;

            foreach (var row in Rows)
            {
                if (row.FindDataPathBySchemaPath(schemaPath, out dataPath))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Validate(JToken formData, JToken schema)
        {
            return ValidateElements(formData, schema, Rows);
        }
    }
}