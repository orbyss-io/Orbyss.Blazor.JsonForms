
namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public class ListItemFormComponentInstance(Type componentType) : ListItemFormComponentInstanceBase
    {
        public override Type ComponentType => componentType;
    }

    public class ListItemFormComponentInstance<TComponent> : ListItemFormComponentInstance
    {
        public ListItemFormComponentInstance() : base(typeof(TComponent))
        {            
        }
    }
}
