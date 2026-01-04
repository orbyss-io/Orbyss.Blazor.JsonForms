using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Interpretation;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context.Utils;

public static class JsonFormDataContextBuilder
{
    public static IJsonFormDataContext Build(
        IJsonTransformer? jsonTransformer = null,
        IJsonPathInterpreter? jsonPathInterpreter = null,
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