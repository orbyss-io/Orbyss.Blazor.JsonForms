using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Interpretation;

namespace Orbyss.Blazor.JsonForms.Context.Models;

public sealed class FormHorizontalLayoutContext(UiSchemaHorizontalLayoutInterpretation interpretation, IEnumerable<IFormElementContext> columns)
    : FormElementContextBase<UiSchemaHorizontalLayoutInterpretation>(interpretation)
{
    public IEnumerable<IFormElementContext> Columns { get; } = columns;

    public override bool ReadOnly => false;

    protected override bool DisabledCore => false;

    protected override bool HiddenCore => false;

    public override bool FindDataPathBySchemaPath(string schemaPath, out string dataPath)
    {
        dataPath = string.Empty;

        foreach (var column in Columns)
        {
            if (column.FindDataPathBySchemaPath(schemaPath, out dataPath))
            {
                return true;
            }
        }

        return false;
    }

    public override bool Validate(JToken formData, JToken schema)
    {
        return ValidateElements(formData, schema, Columns);
    }
}