using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Interpretation.Utils
{
    public static class UiSchemaInterpreterBuilder
    {
        public static IFormUiSchemaInterpreter Build(IJsonPathInterpreter? jsonPathInterpreter = null, IControlTypeInterpreter? controlTypeInterpreter = null)
        {
            return new FormUiSchemaInterpreter(
                jsonPathInterpreter ?? JsonPathInterpreter.Default,
                controlTypeInterpreter ?? new ControlTypeInterpreter()
            );
        }
    }
}