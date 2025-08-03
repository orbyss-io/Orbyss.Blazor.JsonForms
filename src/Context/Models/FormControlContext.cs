using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Interpretation;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed class FormControlContext(string absoluteDataPath, string? absoluteParentDataPath, UiSchemaControlInterpretation interpretation)
        : FormControlContextBase<UiSchemaControlInterpretation>(interpretation, absoluteDataPath, absoluteParentDataPath)
    {
        public override bool FindDataPathBySchemaPath(string schemaPath, out string dataPath)
        {
            dataPath = string.Empty;
            var result = Interpretation.AbsoluteSchemaJsonPath == schemaPath;

            if (result)
            {
                dataPath = AbsoluteDataJsonPath;
            }

            return result;
        }

        public override bool Validate(JToken formData, JToken schema)
        {
            var errors = new List<ErrorType>();

            var isRequiredControl = GetIsPropertyRequired(formData, schema, AbsoluteParentObjectDataJsonPath, out var parentData);

            var controlSchemaToken = schema.SelectToken(Interpretation.AbsoluteSchemaJsonPath, true);
            var controlSchema = JSchema.Parse($"{controlSchemaToken}");
            var controlData = formData.SelectToken(AbsoluteDataJsonPath, false);

            if (isRequiredControl && (controlData is null || parentData is null))
            {
                errors.Add(ErrorType.Required);
            }
            else if (controlData is not null)
            {
                _ = controlData.IsValid(controlSchema, out IList<ValidationError> validationErrors);
                errors.AddRange(
                    validationErrors.Select(x => x.ErrorType)
                );
            }

            Errors = [.. errors];

            return !Errors.Any();
        }
    }
}
