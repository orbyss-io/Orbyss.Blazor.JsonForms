using Orbyss.Components.Json.Models;
using System.Text.Json.Serialization;

namespace Orbyss.Components.JsonForms.Context.Translations
{    
    public sealed record TranslationSection(        
        string? Label,
        TranslationErrorSection? Error,
        IEnumerable<TranslatedEnumItem>? Enums,
        IDictionary<string, TranslationSection>? NestedSections
    );
}
