namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class NavigationFormComponentInstance(Type componentType) : NavigationFormComponentInstanceBase
    {
        public override sealed Type ComponentType => componentType;
    }

    public class NavigationFormComponentInstance<TComponent> : NavigationFormComponentInstance
    {
        public NavigationFormComponentInstance() : base(typeof(TComponent))
        {
        }
    }
}