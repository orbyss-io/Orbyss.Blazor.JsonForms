using Orbyss.Components.JsonForms.Interpretation.Interfaces;
using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public class UiSchemaListInterpretation(
        UiSchemaLabelInterpretation? labelInterpretation,
        bool readOnly,
        bool disabled,
        bool hidden,
        string relativeSchemaJsonPath,
        string absoluteSchemaJsonPath,
        string relativeItemsSchemaJsonPath,
        string absoluteItemsSchemaJsonPath,
        string listJsonPropertyName,
        string? absoluteParentObjectSchemaPath,
        FormUiSchemaElement element,
        UiSchemaRuleInterpretation? rule)

        : UiSchemaControlInterpretationBase(labelInterpretation, readOnly, disabled, hidden, relativeSchemaJsonPath, absoluteSchemaJsonPath, listJsonPropertyName, absoluteParentObjectSchemaPath, element, rule)
    {
        private IUiSchemaElementInterpretation? listDetails;

        public override UiSchemaElementInterpretationType ElementType => UiSchemaElementInterpretationType.List;

        public string AbsoluteItemsSchemaJsonPath { get; } = absoluteItemsSchemaJsonPath;

        public string ListJsonPropertyName { get; } = listJsonPropertyName;

        public string RelativeItemsSchemaJsonPath { get; } = relativeItemsSchemaJsonPath;

        public IUiSchemaElementInterpretation GetListDetail()
        {
            if (listDetails is null)
            {
                throw new ArgumentException("List details is not set for this list");
            }
            return listDetails;
        }

        internal void SetListDetail(IUiSchemaElementInterpretation listDetails)
        {
            if (this.listDetails is not null)
            {
                throw new InvalidOperationException("List details are already set.");
            }

            this.listDetails = listDetails;
        }
    }
}