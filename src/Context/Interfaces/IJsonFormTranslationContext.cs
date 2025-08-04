using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Blazor.JsonForms.Interpretation;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces
{
    public interface IJsonFormTranslationContext
    {
        void Instantiate(TranslationSchema translationSchema, JSchema dataSchema);

        string TranslateErrors(string? language, IEnumerable<ErrorType> errors, UiSchemaControlInterpretationBase controlInterpretation);

        string? TranslateLabel(string? language, UiSchemaLabelInterpretation labelInterpretation);

        string? TranslateLabel(string? language, UiSchemaControlInterpretationBase controlInterpretation);

        IEnumerable<TranslatedEnumItem>? TranslateEnum(string? language, UiSchemaControlInterpretation controlInterpretation);
    }
}