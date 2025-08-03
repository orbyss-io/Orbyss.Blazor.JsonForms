using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Context.Utils
{
    public static class JsonFormDataContextBuilder
    {
        public static IJsonFormDataContext Build(
            IJsonTransformer? jsonTransformer = null ,
            IJsonPathInterpreter? jsonPathInterpreter = null ,
            IFormElementContextFactory? formContextFactory = null)
        {
            jsonPathInterpreter ??= JsonPathInterpreter.Default;
            formContextFactory ??= new FormElementContextFactory(jsonPathInterpreter);
            jsonTransformer ??= new JlioJsonTransformer();
            return new JsonFormDataContext(
                jsonTransformer,
                formContextFactory,
                jsonPathInterpreter
            );
        }

        public static IJsonFormDataContext BuildAndInstantiate(
            JSchema jsonSchema,
            JToken? formData = null,
            IJsonTransformer? jsonTransformer = null,
            IJsonPathInterpreter? jsonPathInterpreter = null,
            IFormElementContextFactory? formContextFactory = null)
        {

            var result = Build(jsonTransformer, jsonPathInterpreter, formContextFactory);
            result.Instantiate(formData ?? new JObject(), jsonSchema);

            return result;
        }
    }
}
