using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Interpretation.Builders
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
