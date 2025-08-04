namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class FormComponentInstance(Type componentType) : FormComponentInstanceBase
    {
        public override sealed Type ComponentType { get; } = componentType;
    }

    public class FormComponentInstance<TComponent> : FormComponentInstance
        where TComponent : class
    {
        public FormComponentInstance() : base(typeof(TComponent))
        {
        }
    }
}