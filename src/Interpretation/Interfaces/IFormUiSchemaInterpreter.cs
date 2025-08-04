using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.UiSchema;

namespace Orbyss.Blazor.JsonForms.Interpretation.Interfaces
{
    public interface IFormUiSchemaInterpreter
    {
        UiSchemaInterpretation Interpret(FormUiSchema uiSchema, JSchema jsonSchema);
    }
}