using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Interpretation;

public sealed class UiSchemaVerticalLayoutInterpretation(UiSchemaLabelInterpretation? labelInterpretation)
    : UiSchemaElementInterpretationBase(labelInterpretation)
{
    public override UiSchemaElementInterpretationType ElementType => UiSchemaElementInterpretationType.VerticalLayout;

    public IUiSchemaElementInterpretation[] Rows { get; private set; } = [];

    internal void SetRows(IUiSchemaElementInterpretation[] rows)
    {
        if (Rows.Length > 0)
        {
            throw new InvalidOperationException("Rows are already set");
        }

        Rows = rows;
    }
}