namespace Orbyss.Components.JsonForms.Interpretation.Interfaces
{
    public interface IUiSchemaElementInterpretation
    {
        UiSchemaLabelInterpretation? Label { get; }

        UiSchemaElementInterpretationType ElementType { get; }
    }
}
