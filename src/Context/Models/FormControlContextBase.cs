using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Interpretation;

namespace Orbyss.Blazor.JsonForms.Context.Models;

public abstract class FormControlContextBase<TInterpretation>(TInterpretation interpretation, string absoluteDataJsonPath, string? absoluteParentDataJsonPath)
    : FormElementContextBase<TInterpretation>(interpretation)
    where TInterpretation : UiSchemaControlInterpretationBase
{
    public string AbsoluteDataJsonPath => absoluteDataJsonPath;

    public IEnumerable<ErrorType> Errors { get; protected set; } = [];

    public string? AbsoluteParentObjectDataJsonPath { get; } = absoluteParentDataJsonPath;

    protected override sealed bool DisabledCore => Interpretation.Disabled;

    protected override bool HiddenCore => Interpretation.Hidden;

    public override sealed bool ReadOnly => Interpretation.ReadOnly;

    protected bool GetIsPropertyRequired(JToken formData, JToken schema, string? absoluteParentDataJsonPath, out JToken? parentData)
    {
        parentData = null;

        if (!string.IsNullOrWhiteSpace(absoluteParentDataJsonPath))
        {
            if (string.IsNullOrWhiteSpace(Interpretation.AbsoluteParentSchemaJsonPath))
            {
                throw new InvalidOperationException(
                    $"The control interpretation is not provided with an absolute parent schema json path; although this control's absolute parent data path is specified"
                );
            }

            var parentSchemaToken = schema.SelectToken(Interpretation.AbsoluteParentSchemaJsonPath, true);
            var parentSchema = JSchema.Parse($"{parentSchemaToken}");

            parentData = formData.SelectToken(absoluteParentDataJsonPath, false);

            return parentSchema.Required.Contains(Interpretation.JsonPropertyName);
        }

        return false;
    }
}