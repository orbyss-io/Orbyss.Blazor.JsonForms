using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Components.JsonForms.UiSchema;
using Orbyss.Components.JsonForms.Utils;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed class JsonFormContextInitOptions
    {
        public JsonFormContextInitOptions(JSchema dataSchema, FormUiSchema uiSchema, TranslationSchema translationSchema)
        {
            DataSchema = dataSchema;
            UiSchema = uiSchema;
            TranslationSchema = translationSchema;
        }

        public JsonFormContextInitOptions(string dataSchemaJson, string uiSchemaJson, string translationSchemaJson)
        {
            DataSchema = JSchema.Parse(dataSchemaJson);
            UiSchema = DefaultJsonConverter.Deserialize<FormUiSchema>(uiSchemaJson);
            TranslationSchema = DefaultJsonConverter.Deserialize<TranslationSchema>(translationSchemaJson);
        }

        public JSchema DataSchema { get; }
        public FormUiSchema UiSchema { get; }
        public TranslationSchema TranslationSchema { get; }
        public JToken? Data { get; init; }
        public string? Language { get; init; }
        public bool Disabled { get; init; }
        public bool ReadOnly { get; init; }
    }
}