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
            IFormContextFactory? formContextFactory = null)
        {
            jsonPathInterpreter ??= JsonPathInterpreter.Default;
            formContextFactory ??= new FormContextFactory(jsonPathInterpreter);
            jsonTransformer ??= new JlioJsonTransformer();
            return new JsonFormDataContext(
                jsonTransformer,
                formContextFactory,
                jsonPathInterpreter
            );
        }
    }
}
