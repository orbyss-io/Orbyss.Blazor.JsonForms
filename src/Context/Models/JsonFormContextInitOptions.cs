using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed record JsonFormContextInitOptions(
        JSchema DataSchema, 
        FormUiSchema UiSchema, 
        TranslationSchema TranslationSchema, 
        JToken? Data = null, 
        string? Language = null, 
        bool Disabled = false, 
        bool ReadOnly = false
    );
}
