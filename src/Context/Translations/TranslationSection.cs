using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.Context.Translations
{
    public sealed record TranslationSection(
        string? Label,
        TranslationErrorSection? Error,
        IEnumerable<TranslatedEnumItem>? Enums,
        IDictionary<string, TranslationSection>? NestedSections
    );
}