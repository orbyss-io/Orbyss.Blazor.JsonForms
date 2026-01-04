using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context.Models;

public abstract class FormElementContextBase<TInterpretation>(TInterpretation interpretation) : IFormElementContext
    where TInterpretation : class, IUiSchemaElementInterpretation
{
    private readonly Guid id = Guid.NewGuid();
    private bool? disabledOverwrite;
    private bool? hiddenOverwrite;

    public Guid Id => id;

    public TInterpretation Interpretation { get; } = interpretation;

    IUiSchemaElementInterpretation IFormElementContext.Interpretation => Interpretation;

    public bool Disabled => disabledOverwrite ?? DisabledCore;

    protected abstract bool DisabledCore { get; }

    protected abstract bool HiddenCore { get; }

    public abstract bool ReadOnly { get; }

    public bool Hidden => hiddenOverwrite ?? HiddenCore;

    protected bool ValidateElements(JToken formData, JToken schema, IEnumerable<IFormElementContext> elements)
    {
        bool isValid = true;
        foreach (var element in elements)
        {
            if (!element.Validate(formData, schema))
            {
                isValid = false;
            }
        }
        return isValid;
    }

    public abstract bool Validate(JToken formData, JToken schema);

    public abstract bool FindDataPathBySchemaPath(string schemaPath, out string dataPath);

    public void SetHidden(bool? value)
    {
        hiddenOverwrite = value;
    }

    public void SetDisabled(bool? value)
    {
        disabledOverwrite = value;
    }
}