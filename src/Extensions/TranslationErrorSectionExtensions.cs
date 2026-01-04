using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.Extensions;

public static class TranslationErrorSectionExtensions
{
    public static IEnumerable<string> GetTranslatedErrors(this TranslationErrorSection errorSection, IEnumerable<ErrorType> types)
    {
        foreach (var type in types)
        {
            var error = GetTranslatedError(errorSection, type);
            if (!string.IsNullOrWhiteSpace(error))
            {
                yield return error;
            }
        }
    }

    public static string GetTranslatedError(this TranslationErrorSection errorSection, ErrorType type)
    {
        return type switch
        {
            ErrorType.Required => errorSection.GetRequired(),
            ErrorType.Minimum => errorSection.GetMinimum(),
            ErrorType.Maximum => errorSection.GetMaximum(),
            ErrorType.MaximumLength => errorSection.GetMaximumLength(),
            ErrorType.MinimumLength => errorSection.GetMinimumLength(),
            ErrorType.MaximumItems => errorSection.GetMaximumItems(),
            ErrorType.MinimumItems => errorSection.GetMinimumItems(),
            ErrorType.Pattern => errorSection.GetPattern(),
            ErrorType.Contains => errorSection.GetContains(),
            ErrorType.Const => errorSection.GetConst(),

            _ => string.Empty,
        };
    }
}
