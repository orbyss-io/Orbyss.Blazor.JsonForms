using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Context.Models;

namespace Orbyss.Components.JsonForms.Context.Interfaces
{
    public interface IJsonFormDataContext
    {
        JSchema GetJsonSchema();

        void Instantiate(JToken formData, JSchema dataSchema);

        bool Validate(IEnumerable<IFormElementContext> contexts);

        JToken? GetValue(FormControlContext formControlContext);

        void UpdateValue(FormControlContext formControlContext, JToken? value);

        JToken GetFormData();

        void AddListItem(FormListContext listContext);

        void RemoveListItem(FormListContext listContext, IFormElementContext listItemContext);

        void InstantiateList(FormListContext listContext);        
    }
}
