using Orbyss.Blazor.JsonForms.UiSchema;

namespace Orbyss.Blazor.JsonForms.Interpretation;

public sealed record UiSchemaLabelInterpretation(string? Label, string? I18n)
{
    public static explicit operator UiSchemaLabelInterpretation(FormUiSchemaElement element)
    {
        if (string.IsNullOrWhiteSpace(element.I18n) && string.IsNullOrWhiteSpace(element.Label))
        {
            var propertyName = element.Scope?.Split('/').Last();
            return new UiSchemaLabelInterpretation(propertyName, null);
        }

        return new UiSchemaLabelInterpretation(element.Label, element.I18n);
    }
}