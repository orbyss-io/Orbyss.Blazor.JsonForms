using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Interpretation.Interfaces
{
    public interface IFormUiSchemaInterpreter
    {
        UiSchemaInterpretation Interpret(FormUiSchema uiSchema, JSchema jsonSchema);
    }
}