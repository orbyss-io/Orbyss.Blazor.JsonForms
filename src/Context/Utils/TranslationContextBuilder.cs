using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Helpers;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Context.Builders
{
    public static class TranslationContextBuilder
    {
        public static IJsonFormTranslationContext Build(IJsonPathInterpreter jsonPathInterpreter, TranslationSchema translationSchema, JSchema dataSchema)
        {
            var result = new JsonFormTranslationContext(
                jsonPathInterpreter
            );
            result.Instantiate(translationSchema, dataSchema);
            return result;
        }

        public static IJsonFormTranslationContext Build(TranslationSchema translationSchema, JSchema jsonSchema)
            => Build(JsonPathInterpreter.Default, translationSchema, jsonSchema);

        public static IJsonFormTranslationContext Build(string translationSchemaJson, string jsonSchema)
            => Build(JsonPathInterpreter.Default, translationSchemaJson, jsonSchema);

        public static IJsonFormTranslationContext Build(IJsonPathInterpreter jsonPathInterpreter, string translationSchemaJson, string jsonSchema)
        {
            return Build(
                jsonPathInterpreter,
                DefaultJsonConverter.Deserialize<TranslationSchema>(translationSchemaJson), 
                JSchema.Parse(jsonSchema)
            );
        }
    }
}
