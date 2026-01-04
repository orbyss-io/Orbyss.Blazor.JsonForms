using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces;

public interface IFormElementContext
{
    Guid Id { get; }

    bool Validate(JToken formData, JToken schema);

    IUiSchemaElementInterpretation Interpretation { get; }

    bool FindDataPathBySchemaPath(string schemaPath, out string dataPath);

    bool Disabled { get; }

    bool ReadOnly { get; }

    bool Hidden { get; }

    void SetHidden(bool? value);

    void SetDisabled(bool? value);
}