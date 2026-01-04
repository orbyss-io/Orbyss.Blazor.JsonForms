using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Context.Models;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces;

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

    JToken? GetValue(Guid controlContextId);

    void UpdateValue(Guid controlContextId, JToken? value);

    JToken GetFormData();

    void UpdateFormData(Action<JToken> updateDelegate);

    string? GetDataContextError(Guid controlContextId);

    string? GetLabel(Guid contextId);

    IEnumerable<TranslatedEnumItem> GetTranslatedEnumItems(Guid controlContextId);

    FormPageContext GetPage(int index);

    void InstantiateList(Guid listContextId);

    void AddListItem(Guid listContextId);

    void RemoveListItem(Guid listContextId, Guid listItemContextId);

    void ChangeLanguage(string language);

    void ChangeDisabled(bool disabled);

    void ChangeReadOnly(bool readOnly);
}