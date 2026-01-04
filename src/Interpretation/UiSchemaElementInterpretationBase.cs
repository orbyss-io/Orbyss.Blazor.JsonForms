using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Interpretation;

public abstract class UiSchemaElementInterpretationBase(UiSchemaLabelInterpretation? label)
    : IUiSchemaElementInterpretation
{
    public UiSchemaLabelInterpretation? Label { get; } = label;

    public abstract UiSchemaElementInterpretationType ElementType { get; }
}