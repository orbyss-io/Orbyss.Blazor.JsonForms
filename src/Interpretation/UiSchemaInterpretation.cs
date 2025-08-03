using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class UiSchemaInterpretation(FormUiSchema uiSchema, UiSchemaPageInterpretation[] pages)
    {
        public UiSchemaInterpretation(FormUiSchema uiSchema, UiSchemaPageInterpretation page)
            : this(uiSchema, [page])
        { }

        public FormUiSchema FormUiSchema { get; } = uiSchema;

        public UiSchemaPageInterpretation[] Pages { get; } = pages;
    }
}