using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Interpretation;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;
using Orbyss.Blazor.JsonForms.Utils;

namespace Orbyss.Blazor.JsonForms.Context.Utils
{
    public static class JsonFormTranslationContextBuilder
    {
        public static IJsonFormTranslationContext BuildAndInstantiate(TranslationSchema translationSchema, JSchema dataSchema, IJsonPathInterpreter jsonPathInterpreter)
        {
            var result = new JsonFormTranslationContext(
                jsonPathInterpreter
            );
            result.Instantiate(translationSchema, dataSchema);
            return result;
        }

        public static IJsonFormTranslationContext BuildAndInstantiate(TranslationSchema translationSchema, JSchema jsonSchema)
            => BuildAndInstantiate(translationSchema, jsonSchema, JsonPathInterpreter.Default);

        public static IJsonFormTranslationContext BuildAndInstantiate(string translationSchemaJson, string jsonSchema)
            => BuildAndInstantiate(translationSchemaJson, jsonSchema, JsonPathInterpreter.Default);

        public static IJsonFormTranslationContext BuildAndInstantiate(string translationSchemaJson, string jsonSchema, IJsonPathInterpreter jsonPathInterpreter)
        {
            return BuildAndInstantiate(
                DefaultJsonConverter.Deserialize<TranslationSchema>(translationSchemaJson),
                JSchema.Parse(jsonSchema),
                jsonPathInterpreter
            );
        }
    }
}