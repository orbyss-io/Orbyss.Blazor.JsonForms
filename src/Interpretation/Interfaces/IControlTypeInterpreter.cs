using Newtonsoft.Json.Schema;

namespace Orbyss.Components.JsonForms.Interpretation.Interfaces
{
    public interface IControlTypeInterpreter
    {
        ControlType Interpret(JSchema jsonSchema, string absoluteControlJsonSchemaPath, string? absoluteControlParentSchemaJsonPath);
    }
}
