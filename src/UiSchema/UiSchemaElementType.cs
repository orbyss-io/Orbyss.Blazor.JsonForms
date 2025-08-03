using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Orbyss.Components.JsonForms.UiSchema
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UiSchemaElementType
    {
        VerticalLayout,
        HorizontalLayout,
        Categorization,
        Category,
        Group,
        Control,
        ListWithDetail
    }
}
