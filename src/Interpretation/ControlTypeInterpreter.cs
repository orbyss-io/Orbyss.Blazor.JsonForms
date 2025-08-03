using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;
using Unfussiness.Components.JsonForms.Context.Exceptions;

namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class ControlTypeInterpreter : IControlTypeInterpreter
    {
        public ControlType Interpret(JSchema jsonSchema, string absoluteJsonSchemaPath, string? absoluteParentSchemaJsonPath)
        {
            var jsonSchemaToken = JToken.Parse($"{jsonSchema}");
            var schemaToken = jsonSchemaToken?.SelectToken(absoluteJsonSchemaPath, false)
                ?? throw new InvalidSchemaTypeConfigurationException("Data schema is null");

            var parentSchemaToken = !string.IsNullOrWhiteSpace(absoluteParentSchemaJsonPath)
                ? jsonSchemaToken?.SelectToken(absoluteParentSchemaJsonPath, false)
                : null;
            var parentSchema = parentSchemaToken is not null
                ? JSchema.Parse($"{parentSchemaToken}")
                : null;

            var schema = JSchema.Parse($"{schemaToken}");

            if (!schema.Type.HasValue)
            {
                throw new SchemaTypeNotSpecifiedException($"Schema type is not specified.");
            }

            if (schema.Type.Value.HasFlag(JSchemaType.String))
            {
                return HandleStringType(schema, parentSchema);
            }
            if (schema.Type.Value.HasFlag(JSchemaType.Number))
            {
                return HandleNumericType(schema);
            }
            if (schema.Type.Value.HasFlag(JSchemaType.Integer))
            {
                return ControlType.Integer;
            }
            if (schema.Type.Value.HasFlag(JSchemaType.Boolean))
            {
                return ControlType.Boolean;
            }
            if (schema.Type.Value.HasFlag(JSchemaType.Array))
            {
                return HandleArrayType(schema);
            }

            throw new SchemaTypeNotSupportedException($"Schema type '{schema.Type}' is not supported");
        }

        private static ControlType HandleArrayType(JSchema schema)
        {
            if (!schema.Items.Any())
            {
                throw new InvalidSchemaTypeConfigurationException($"The schema type is '{schema.Type}', but no 'items' are specified.");
            }
            if (schema.Items.Count > 1)
            {
                throw new InvalidSchemaTypeConfigurationException($"The schema type is '{schema.Type}', but more than one 'items' are specified. Only 1 is supported.");
            }

            var itemsSchema = schema.Items.First();

            // ARGUE THIS: WE CAN HAVE AN ARRAY OF NUMBERS< ENUMS< etc. We need to render it as a dropdown. Only with objects, we need to have a list.
            if (itemsSchema.Type != JSchemaType.String)
            {
                throw new InvalidSchemaTypeConfigurationException($"The items schema of array is '{itemsSchema.Type}', however only '{JSchemaType.String}' is supported in the array control type.");
            }

            return HandleStringType(itemsSchema, schema);
        }

        private static ControlType HandleStringType(JSchema schema, JSchema? parentSchema)
        {
            var format = schema.Format?.ToLower();
            var enumItems = schema.Enum.Select(x => x.ToString());
            var isArray = parentSchema?.Type?.HasFlag(JSchemaType.Array) == true;

            if (enumItems.Any())
            {
                return isArray
                    ? ControlType.EnumList
                    : ControlType.Enum;
            }
            if (format == "datetime")
            {
                return ControlType.DateTime;
            }
            if (format == "date")
            {
                return ControlType.DateOnly;
            }

            return ControlType.String;
        }

        private static ControlType HandleNumericType(JSchema schema)
        {
            var format = schema.Format?.ToLower();

            if (format == "datetime")
            {
                return ControlType.DateTimeUtcTicks;
            }
            if (format == "date")
            {
                return ControlType.DateOnlyUtcTicks;
            }

            return ControlType.Number;
        }
    }
}
