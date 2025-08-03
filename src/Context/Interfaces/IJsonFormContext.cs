using Newtonsoft.Json.Linq;
using Orbyss.Components.JsonForms.Context.Models;

namespace Orbyss.Components.JsonForms.Context.Interfaces
{
    public interface IJsonFormContext
    {
        IEnumerable<FormPageContext> GetPages();

        string? ActiveLanguage { get; }

        IJsonFormNotification FormNotification { get; }

        int PageCount { get; }

        bool Disabled { get; }

        bool ReadOnly { get; }

        void Instantiate(JsonFormContextInitOptions initOptions);

        JToken? GetFormOption(string key);

        bool Validate(Guid? pageId = null);

        JToken? GetValue(Guid dataContextId);

        void UpdateValue(Guid dataContextId, JToken? value);

        string? GetDataContextError(Guid dataContextId);

        string? GetLabel(Guid contextId);

        FormPageContext GetPage(int index);

        void InstantiateList(Guid listContextId);

        void ChangeLanguage(string language);

        void ChangeDisabled(bool disabled);

        void ChangeReadOnly(bool readOnly);
    }
}
