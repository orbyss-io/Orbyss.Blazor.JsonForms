using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class UiSchemaHorizontalLayoutInterpretation() 
        : UiSchemaElementInterpretationBase(null)
    {
        public override UiSchemaElementInterpretationType ElementType => UiSchemaElementInterpretationType.HorizontalLayout;

        public IUiSchemaElementInterpretation[] Columns { get; private set; } = [];

        internal void SetColumns(IUiSchemaElementInterpretation[] columns)
        {
            if (Columns.Length > 0)
            {
                throw new InvalidOperationException("Columns are already set");
            }

            Columns = columns;
        }
    }
}
