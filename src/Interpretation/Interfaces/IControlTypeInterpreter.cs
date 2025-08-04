using Newtonsoft.Json.Schema;

namespace Orbyss.Blazor.JsonForms.Interpretation.Interfaces
{
    public interface IControlTypeInterpreter
    {
        ControlType Interpret(JSchema jsonSchema, string absoluteControlJsonSchemaPath, string? absoluteControlParentSchemaJsonPath);
    }
}