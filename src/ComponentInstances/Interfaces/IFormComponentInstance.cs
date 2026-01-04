namespace Orbyss.Blazor.JsonForms.ComponentInstances.Interfaces;

public interface IFormComponentInstance
{
    IDictionary<string, object?> GetParameters();

    Type ComponentType { get; }
}