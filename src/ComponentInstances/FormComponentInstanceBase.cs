using Orbyss.Blazor.JsonForms.ComponentInstances.Interfaces;
using Orbyss.Blazor.JsonForms.Constants;
using Orbyss.Blazor.JsonForms.Utils;
using System.Globalization;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public abstract class FormComponentInstanceBase : IFormComponentInstance
{
    public abstract Type ComponentType { get; }

    public string? Class { get; set; }

    public string? Style { get; set; }

    public CultureInfo Culture { get; set; } = FormCulture.Instance;

    public IDictionary<string, object?> GetParameters()
    {
        var result = GetParametersCore();
        result[nameof(Class)] = Class;
        result[nameof(Style)] = Style;
        result[nameof(Culture)] = Culture;

        RemoveUndeclaredParameters.Remove(ComponentType, result);

        return result;
    }

    protected virtual IDictionary<string, object?> GetParametersCore()
    {
        return new Dictionary<string, object?>();
    }

    protected static void AddIfNotContains(IDictionary<string, object?> dictionary, string? key, object? value)
    {
        if (!string.IsNullOrWhiteSpace(key) && !dictionary.ContainsKey(key) && value is not null)
        {
            dictionary.Add(key, value);
        }
    }
}