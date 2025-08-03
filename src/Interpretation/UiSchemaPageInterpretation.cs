using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class UiSchemaPageInterpretation(
        bool readOnly,
        bool disabled,
        bool hidden,
        IUiSchemaElementInterpretation[] interpretedElements,
        UiSchemaLabelInterpretation? labelInterpretation,
        UiSchemaRuleInterpretation? rule)
    {
        public UiSchemaPageInterpretation(
            bool readOnly,
            bool disabled,
            bool hidden,
            IUiSchemaElementInterpretation interpretedElements,
            UiSchemaLabelInterpretation? labelInterpretation,
            UiSchemaRuleInterpretation? rule)

            : this(readOnly, disabled, hidden, [interpretedElements], labelInterpretation, rule)
        {
        }

        public IUiSchemaElementInterpretation[] InterpretedElements { get; } = interpretedElements;

        public UiSchemaLabelInterpretation? LabelInterpretation { get; } = labelInterpretation;

        public UiSchemaRuleInterpretation? Rule { get; } = rule;

        public bool ReadOnly => readOnly;

        public bool Disabled => disabled;

        public bool Hidden => hidden;
    }
}
