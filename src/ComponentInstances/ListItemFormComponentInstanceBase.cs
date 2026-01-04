using Microsoft.AspNetCore.Components;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public abstract class ListItemFormComponentInstanceBase : FormComponentInstanceBase
{
    public bool Disabled { get; internal set; }

    public EventCallback OnRemoveItemClicked { get; internal set; }

    protected override sealed IDictionary<string, object?> GetParametersCore()
    {
        var result = GetListItemParameters();

        result[nameof(Disabled)] = Disabled;
        result[nameof(OnRemoveItemClicked)] = OnRemoveItemClicked;

        return result;
    }

    protected virtual IDictionary<string, object?> GetListItemParameters()
    {
        return new Dictionary<string, object?>();
    }
}