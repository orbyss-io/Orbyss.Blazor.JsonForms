using Newtonsoft.Json.Linq;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public abstract class DropdownFormComponentInstanceBase : InputFormComponentInstanceBase
{
    public bool Clearable { get; set; }

    public bool Searchable { get; set; }

    public bool MultiSelect { get; internal set; }

    public IEnumerable<TranslatedEnumItem> Items { get; internal set; } = [];

    protected override sealed object? ConvertValue(JToken? value)
    {
        if (MultiSelect)
        {
            return value?.ToObject<IEnumerable<string>>();
        }

        return value?.ToString();
    }

    protected override sealed IDictionary<string, object?> GetFormInputParameters()
    {
        var result = GetDropdownParameters();

        AddIfNotContains(result, nameof(Clearable), Clearable);
        AddIfNotContains(result, nameof(Searchable), Searchable);

        result[nameof(MultiSelect)] = MultiSelect;
        result[nameof(Items)] = Items;

        return result;
    }

    protected virtual IDictionary<string, object?> GetDropdownParameters()
    {
        return new Dictionary<string, object?>();
    }
}