using Orbyss.Components.Json.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orbyss.Components.JsonForms.Context.Translations
{
    public sealed class TranslationSectionJsonConverter : JsonConverter<TranslationSection>
    {
        public override TranslationSection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var label = string.Empty;
            var error = default(TranslationErrorSection);
            var enumItems = new List<TranslatedEnumItem>();
            var nestedSections = new Dictionary<string, TranslationSection>();

            if (reader.TokenType == JsonTokenType.String)
            {
                label = reader.GetString();
                return new TranslationSection(
                    label,
                    null,
                    enumItems,
                    nestedSections
                );
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name");
                }

                var propertyName = reader.GetString()!;

                if (!reader.Read())
                {
                    throw new JsonException("Expected property value");
                }

                if (propertyName.Equals(nameof(TranslationSection.Label), StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException($"Expected string value for property but was '{reader.TokenType}'");
                    }

                    label = reader.GetString();
                    continue;
                }

                if (propertyName.Equals(nameof(TranslationSection.Error), StringComparison.OrdinalIgnoreCase))
                {
                    error = JsonSerializer.Deserialize<TranslationErrorSection>(ref reader, options);
                    continue;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var value = reader.GetString();
                    var enumItem = new TranslatedEnumItem(value ?? string.Empty, propertyName);
                    enumItems.Add(enumItem);
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var nestedSection = Read(ref reader, typeToConvert, options);
                    if (nestedSection is not null)
                    {
                        nestedSections.Add(propertyName, nestedSection);
                    }
                    continue;
                }
            }

            return new TranslationSection(
                label, error, enumItems, nestedSections
            );
        }

        public override void Write(Utf8JsonWriter writer, TranslationSection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("label");
            writer.WriteStringValue(value.Label);

            writer.WritePropertyName("error");
            if (value.Error is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Error, options);
            }

            if (value.Enums?.Any() == true)
            {
                foreach (var @enum in value.Enums)
                {
                    writer.WritePropertyName(@enum.Value);
                    writer.WriteStringValue(@enum.Label);
                }
            }

            if (value.NestedSections?.Count > 0)
            {
                foreach (var (key, nestedSection) in value.NestedSections)
                {
                    writer.WritePropertyName(
                        ToCamelCase(key)
                    );

                    Write(writer, nestedSection, options);
                }
            }

            writer.WriteEndObject();
        }

        private static string ToCamelCase(string value)
        {
            var result = value.ToCharArray();
            result[0] = char.ToLower(result[0]);
            return new string(result);
        }
    }
}