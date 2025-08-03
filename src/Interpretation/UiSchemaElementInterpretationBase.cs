using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public abstract class UiSchemaElementInterpretationBase(UiSchemaLabelInterpretation? label)
        : IUiSchemaElementInterpretation
    {
        public UiSchemaLabelInterpretation? Label { get; } = label;

        public abstract UiSchemaElementInterpretationType ElementType { get; }
    }
}
