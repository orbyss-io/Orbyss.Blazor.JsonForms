
namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public class ButtonFormComponentInstance : ButtonFormComponentInstanceBase
{
    public ButtonFormComponentInstance(Type componentType, IDictionary<string, string> textTranslations)
        : base(textTranslations)
    {
        ComponentType = componentType;
    }

    public ButtonFormComponentInstance(Type componentType, string text)
        : base(text)
    {
        ComponentType = componentType;
    }

    public override sealed Type ComponentType { get; }
}

public class ButtonFormComponentInstance<TComponent> : ButtonFormComponentInstance
{
    public ButtonFormComponentInstance(IDictionary<string, string> textTranslations)
        : base(typeof(TComponent), textTranslations)
    {
    }

    public ButtonFormComponentInstance(string text)
        : base(typeof(TComponent), text)
    {
    }
}
