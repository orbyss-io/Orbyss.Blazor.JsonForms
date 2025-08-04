using Newtonsoft.Json.Linq;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class DropdownFormComponentInstance(Type componentType) : DropdownFormComponentInstanceBase
    {
        public override Type ComponentType => componentType;       
    }

    public class DropdownFormComponentInstance<TComponent> : DropdownFormComponentInstance
    {
        public DropdownFormComponentInstance() : base(typeof(TComponent))
        {
        }
    }
}