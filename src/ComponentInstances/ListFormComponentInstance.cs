namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class ListFormComponentInstance(Type componentType) : ListFormComponentInstanceBase
    {
        public override Type ComponentType => componentType;
    }

    public class ListFormComponentInstance<TComponent> : ListFormComponentInstance
    {
        public ListFormComponentInstance() : base(typeof(TComponent))
        {
        }
    }
}