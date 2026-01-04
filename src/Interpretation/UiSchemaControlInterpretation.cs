using Orbyss.Blazor.JsonForms.UiSchema;

namespace Orbyss.Blazor.JsonForms.Interpretation;

public sealed class UiSchemaControlInterpretation(
    ControlType controlType,
    UiSchemaLabelInterpretation labelInterpretation,
    bool readOnly,
    bool disabled,
    bool hidden,
    string relativeSchemaJsonPath,
    string absoluteSchemaJsonPath,
    string controlJsonPropertyName,
    string? absoluteParentObjectSchemaPath,
    FormUiSchemaElement element,
    UiSchemaRuleInterpretation? rule)

    : UiSchemaControlInterpretationBase(labelInterpretation, readOnly, disabled, hidden, relativeSchemaJsonPath, absoluteSchemaJsonPath, controlJsonPropertyName, absoluteParentObjectSchemaPath, element, rule)
{
    public override UiSchemaElementInterpretationType ElementType => UiSchemaElementInterpretationType.Control;

    public ControlType ControlType { get; } = controlType;
}