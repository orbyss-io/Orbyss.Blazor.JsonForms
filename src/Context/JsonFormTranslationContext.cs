using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Translations;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;
using Orbyss.Components.JsonForms.Utils;
using System.Text.Json;

namespace Orbyss.Components.JsonForms.Context
{
    public sealed class JsonFormTranslationContext(IJsonPathInterpreter jsonPathInterpreter)
        : IJsonFormTranslationContext
    {
        static readonly JsonSerializerOptions serializerOptions = GetSerializerOptions();

        private TranslationObject[] translations = [];
        private JObject schema = [];

        public void Instantiate(TranslationSchema translationSchema, JSchema dataSchema)
        {
            if (translations?.Length > 0)
            {
                throw new InvalidOperationException("Translation context is already instantiated.");
            }

            translations = [.. ConvertToTranslationObjects(translationSchema)];

            schema = JObject.Parse(dataSchema.ToString());
        }

        public string TranslateErrors(string? language, IEnumerable<ErrorType> errors, UiSchemaControlInterpretationBase controlInterpretation)
        {
            var translation = GetTranslationObject(language);
            var errorSection = TranslationErrorSection.DefaultSection();

            if (translation is not null)
            {
                var translationSectionPath = jsonPathInterpreter.FromJsonSchemaPath(controlInterpretation.AbsoluteSchemaJsonPath);
                var translationSection = GetSectionByPath(translation, translationSectionPath);
                if (translationSection?.Error is not null)
                {
                    errorSection = translationSection.Error;
                }
            }

            var translatedError = errors
                .Select(errorSection.GetValue)
                .ToArray();

            return string.Join(". ", translatedError).Replace("..", ".");
        }

        public string? TranslateLabel(string? language, UiSchemaLabelInterpretation labelInterpretation)
        {
            return TranslateLabel(language, labelInterpretation, null);
        }

        public string? TranslateLabel(string? language, UiSchemaControlInterpretationBase controlInterpretation)
        {
            return TranslateLabel(language, controlInterpretation.Label, controlInterpretation.AbsoluteSchemaJsonPath);
        }

        public IEnumerable<TranslatedEnumItem>? TranslateEnum(string? language, UiSchemaControlInterpretation controlInterpretation)
        {
            var translation = GetTranslationObject(language);
            if (translation is null)
            {
                return GetDefaultTranslatedEnumItems(controlInterpretation.AbsoluteSchemaJsonPath);
            }

            var translationSectionPath = jsonPathInterpreter.FromJsonSchemaPath(controlInterpretation.AbsoluteSchemaJsonPath);
            var translationSection = GetSectionByPath(translation, translationSectionPath);

            if (translationSection?.Enums?.Any() == true)
            {
                return translationSection.Enums;
            }

            return GetDefaultTranslatedEnumItems(controlInterpretation.AbsoluteSchemaJsonPath);
        }

        IEnumerable<TranslatedEnumItem>? GetDefaultTranslatedEnumItems(string absoluteSchemaJsonPath)
        {
            var enumSchemaSection = schema.SelectToken(absoluteSchemaJsonPath);
            if (enumSchemaSection is null
                || enumSchemaSection is not JObject enumSchemaObject
                || !enumSchemaObject.ContainsKey("enum")
                || enumSchemaObject["enum"] is not JArray enumArray)
            {
                return null;
            }

            return enumArray.Select(x =>
            {
                return new TranslatedEnumItem($"{x}", $"{x}");
            });
        }

        string? TranslateLabel(string? language, UiSchemaLabelInterpretation? labelInterpretation, string? absoluteSchemaPath)
        {
            var propertyName = !string.IsNullOrWhiteSpace(absoluteSchemaPath)
               ? jsonPathInterpreter.GetJsonPropertyNameFromPath(absoluteSchemaPath)
               : string.Empty;

            var translation = GetTranslationObject(language);

            if (string.IsNullOrWhiteSpace(language) || translation is null)
            {
                return propertyName.ToHumanReadableName();
            }

            if (!string.IsNullOrWhiteSpace(labelInterpretation?.I18n)
                 && translation.Sections.TryGetValue(labelInterpretation.I18n, out var i18nSection))
            {
                return i18nSection.Label;
            }

            if (!string.IsNullOrWhiteSpace(labelInterpretation?.Label)
                && propertyName != labelInterpretation.Label
                && translation.Sections.TryGetValue(labelInterpretation.Label, out var simpleLabelSection))
            {
                return simpleLabelSection.Label;
            }

            if (string.IsNullOrWhiteSpace(absoluteSchemaPath))
            {
                return null;
            }

            var translationSectionPath = jsonPathInterpreter.FromJsonSchemaPath(absoluteSchemaPath);
            var translationSection = GetSectionByPath(translation, translationSectionPath);
            if (translationSection is not null)
            {
                return translationSection.Label;
            }

            return propertyName?.ToHumanReadableName();
        }

        TranslationSection? GetSectionByPath(TranslationObject translation, string path)
        {
            var pathElements = jsonPathInterpreter.GetPathElements(path);

            if (pathElements.Length < 1)
            {
                return null;
            }
            if (!translation.Sections.TryGetValue(pathElements[0], out var section) || section is null)
            {
                return null;
            }

            if (pathElements.Length == 1)
            {
                return section;
            }

            if (section.NestedSections is null)
            {
                return null;
            }

            return GetNestedSection(section, pathElements, 1);
        }

        static TranslationSection? GetNestedSection(TranslationSection parent, string[] pathElements, int currentIndex)
        {
            if (parent.NestedSections is null || parent.NestedSections.Count == 0)
            {
                return null;
            }

            var currentPathElement = pathElements[currentIndex];
            if (!parent.NestedSections.TryGetValue(currentPathElement, out var section))
            {
                return null;
            }

            if (currentIndex == (pathElements.Length - 1))
            {
                return section;
            }

            return GetNestedSection(section, pathElements, currentIndex++);
        }

        TranslationObject? GetTranslationObject(string? language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return null;
            }

            return translations.FirstOrDefault(x => x.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
        }

        static IEnumerable<TranslationObject> ConvertToTranslationObjects(TranslationSchema translationSchema)
        {
            return translationSchema.Resources.Select(x => ConvertToTranslationObject(x.Key, x.Value));
        }

        static TranslationObject ConvertToTranslationObject(string language, TranslationSchemaResource resource)
        {
            var json = ObjectJsonConverter.Serialize(resource.Translation);
            var sections = JsonSerializer.Deserialize<Dictionary<string, TranslationSection>>(json, serializerOptions);
            var sectionsWithEqualityComparer = new Dictionary<string, TranslationSection>(
                sections ?? [],
                StringComparer.OrdinalIgnoreCase
            );
            return new TranslationObject(language, sectionsWithEqualityComparer);
        }

        static JsonSerializerOptions GetSerializerOptions()
        {
            var result = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            result.Converters.Add(new TranslationSectionJsonConverter());
            return result;
        }
    }
}
