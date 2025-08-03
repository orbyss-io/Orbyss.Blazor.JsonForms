namespace Orbyss.Components.JsonForms.Context.Translations
{
    public sealed class TranslationObject(string language, IDictionary<string, TranslationSection> sections)
    {
        public string Language { get; } = language;

        public IDictionary<string, TranslationSection> Sections { get; } = sections;
    }
}