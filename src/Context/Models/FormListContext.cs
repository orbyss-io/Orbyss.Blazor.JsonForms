using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Interpretation;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed class FormListContext(UiSchemaListInterpretation interpretation, string absoluteDataJsonPath, string? absoluteParentDataJsonPath)
        : FormControlContextBase<UiSchemaListInterpretation>(interpretation, absoluteDataJsonPath, absoluteParentDataJsonPath)
    {
        private readonly IList<IFormElementContext> items = [];

        public IFormElementContext[] Items => [.. items];

        public void AddItem(IFormElementContext item) => items.Add(item);

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

        public int RemoveItem(Guid itemContextId)
        {
            for (var i = 0; i < Items.Length; i++)
            {
                if (Items[i].Id == itemContextId)
                {
                    items.RemoveAt(i);
                    return i;
                }
            }

            throw new InvalidOperationException($"This list does not contain an item with id '{itemContextId}'");
        }

        public override bool Validate(JToken formData, JToken schema)
        {
            var errors = new List<ErrorType>();

            var listSchemaToken = schema.SelectToken(Interpretation.AbsoluteSchemaJsonPath, true);
            var listSchema = JSchema.Parse($"{listSchemaToken}");

            var listData = formData.SelectToken(AbsoluteDataJsonPath, false);

            var isRequired = GetIsPropertyRequired(formData, schema, AbsoluteParentObjectDataJsonPath, out var parentData);

            if (isRequired && (listData is null || parentData is null))
            {
                errors.Add(ErrorType.Required);
            }
            else if (listData is not null)
            {
                if (listData is not JArray list)
                {
                    throw new InvalidOperationException($"Data error; expected list data, but the data is not a JSON array.");
                }

                if (listSchema.MinimumItems.HasValue && list.Count < listSchema.MinimumItems.Value)
                    errors.Add(ErrorType.MinimumItems);
                if (listSchema.MaximumItems.HasValue && list.Count > listSchema.MaximumItems.Value)
                    errors.Add(ErrorType.MaximumItems);
            }

            Errors = [.. errors];

            var areItemsValid = ValidateElements(formData, schema, Items);

            return areItemsValid && !Errors.Any();
        }
    }
}